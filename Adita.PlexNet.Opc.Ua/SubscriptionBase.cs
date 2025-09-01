// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions;
using Adita.PlexNet.Opc.Ua.Annotations;
using Adita.PlexNet.Opc.Ua.Applications;
using Adita.PlexNet.Opc.Ua.Channels;
using Adita.PlexNet.Opc.Ua.Collections;
using Adita.PlexNet.Opc.Ua.Events;
using Adita.PlexNet.Opc.Ua.Extensions;
using Adita.PlexNet.Opc.Ua.Internal.Extensions;
using Adita.PlexNet.Opc.Ua.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A base class that subscribes to receive data changes and events from an OPC UA server.
    /// </summary>
    public abstract partial class SubscriptionBase : ObservableValidator, ISetDataErrorInfo, IAsyncDisposable
    {
        #region Caching fields
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<MonitoredItemPropertyInfoDescriptor>> _cachedMonitoredItemPropertyInfoDescriptors = [];
        #endregion Caching fields

        #region Private fields
        private bool _disposed;

        private readonly ActionBlock<PublishResponse> _actionBlock;
        private readonly IProgress<CommunicationState> _progress;
        private readonly ILogger? _logger;
        private readonly UaApplication _application;
        private volatile bool _isPublishing;
        private volatile ClientSessionChannel? _innerChannel;
        private volatile uint _subscriptionId;
        private readonly string? _endpointUrl;
        private readonly double _publishingInterval = ClientSessionChannel.DefaultPublishingInterval;
        private readonly uint _keepAliveCount = ClientSessionChannel.DefaultKeepaliveCount;
        private readonly uint _lifetimeCount;
        private readonly MonitoredItemBaseCollection _monitoredItems = [];
        private CommunicationState _state = CommunicationState.Created;
        // private volatile TaskCompletionSource<bool> whenSubscribed;
        //private volatile TaskCompletionSource<bool> whenUnsubscribed;
        private readonly CancellationTokenSource _stateMachineCts;
        private readonly Task _stateMachineTask;

        private readonly Dictionary<string, List<string>> _errors = [];
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionBase"/> class.
        /// </summary>
        protected SubscriptionBase()
            : this(UaApplication.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionBase"/> class.
        /// </summary>
        /// <param name="application">The UaApplication.</param>
        protected SubscriptionBase(UaApplication? application)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
            _application.Completion.ContinueWith(t => _stateMachineCts?.Cancel());
            _logger = _application.LoggerFactory?.CreateLogger(GetType());
            _progress = new Progress<CommunicationState>(s => State = s);
            PropertyChanged += OnPropertyChanged;

            // register the action to be run on the ui thread, if there is one.
            if (SynchronizationContext.Current != null)
            {
                _actionBlock = new ActionBlock<PublishResponse>(pr => OnPublishResponse(pr), new ExecutionDataflowBlockOptions { SingleProducerConstrained = true, TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext() });
            }
            else
            {
                _actionBlock = new ActionBlock<PublishResponse>(pr => OnPublishResponse(pr), new ExecutionDataflowBlockOptions { SingleProducerConstrained = true });
            }

            // read [Subscription] attribute.
            var typeInfo = GetType().GetTypeInfo();
            var sa = typeInfo.GetCustomAttribute<SubscriptionAttribute>();
            if (sa != null)
            {
                _endpointUrl = sa.EndpointUrl;
                _publishingInterval = sa.PublishingInterval;
                _keepAliveCount = sa.KeepAliveCount;
                _lifetimeCount = sa.LifetimeCount;
            }

            // read [MonitoredItem] attributes.
            var monitoredItemProperyInfoDescriptors = GetMonitoredItemPropertyInfoDescriptors(typeInfo);

            foreach (var monitoredItemProperyInfoDescriptor in monitoredItemProperyInfoDescriptors)
            {
                var mia = monitoredItemProperyInfoDescriptor.MonitoredItemAttribute;

                MonitoringFilter? filter = null;
                if (mia.AttributeId == AttributeIds.Value && (mia.DataChangeTrigger != DataChangeTrigger.StatusValue || mia.DeadbandType != DeadbandType.None))
                {
                    filter = new DataChangeFilter() { Trigger = mia.DataChangeTrigger, DeadbandType = (uint)mia.DeadbandType, DeadbandValue = mia.DeadbandValue };
                }

                var propType = monitoredItemProperyInfoDescriptor.PropertyInfo.PropertyType;
                if (propType == typeof(DataValue))
                {
                    _monitoredItems.Add(new DataValueMonitoredItem(
                        target: this,
                        property: monitoredItemProperyInfoDescriptor.PropertyInfo,
                        nodeId: ExpandedNodeId.Parse(mia.NodeId),
                        attributeId: mia.AttributeId,
                        indexRange: mia.IndexRange,
                        samplingInterval: mia.SamplingInterval,
                        filter: filter,
                        queueSize: mia.QueueSize,
                        discardOldest: mia.DiscardOldest));
                    continue;
                }

                if (propType == typeof(BaseEvent) || propType.GetTypeInfo().IsSubclassOf(typeof(BaseEvent)))
                {
                    _monitoredItems.Add(new EventMonitoredItem(
                        target: this,
                        property: monitoredItemProperyInfoDescriptor.PropertyInfo,
                        nodeId: ExpandedNodeId.Parse(mia.NodeId),
                        attributeId: mia.AttributeId,
                        indexRange: mia.IndexRange,
                        samplingInterval: mia.SamplingInterval,
                        filter: new EventFilter() { SelectClauses = EventHelper.GetSelectClauses(propType) },
                        queueSize: mia.QueueSize,
                        discardOldest: mia.DiscardOldest));
                    continue;
                }

                if (propType == typeof(ObservableQueue<DataValue>))
                {
                    _monitoredItems.Add(new DataValueQueueMonitoredItem(
                        target: this,
                        property: monitoredItemProperyInfoDescriptor.PropertyInfo,
                        nodeId: ExpandedNodeId.Parse(mia.NodeId),
                        attributeId: mia.AttributeId,
                        indexRange: mia.IndexRange,
                        samplingInterval: mia.SamplingInterval,
                        filter: filter,
                        queueSize: mia.QueueSize,
                        discardOldest: mia.DiscardOldest));
                    continue;
                }

                if (propType.IsConstructedGenericType && propType.GetGenericTypeDefinition() == typeof(ObservableQueue<>))
                {
                    var elemType = propType.GenericTypeArguments[0];
                    if (elemType == typeof(BaseEvent) || elemType.GetTypeInfo().IsSubclassOf(typeof(BaseEvent)))
                    {
                        _monitoredItems.Add((MonitoredItemBase)Activator.CreateInstance(
                        typeof(EventQueueMonitoredItem<>).MakeGenericType(elemType),
                        this,
                         monitoredItemProperyInfoDescriptor.PropertyInfo,
                        ExpandedNodeId.Parse(mia.NodeId),
                        mia.AttributeId,
                        mia.IndexRange,
                        MonitoringMode.Reporting,
                        mia.SamplingInterval,
                        new EventFilter() { SelectClauses = EventHelper.GetSelectClauses(elemType) },
                        mia.QueueSize,
                        mia.DiscardOldest)!);
                        continue;
                    }
                }

                _monitoredItems.Add((MonitoredItemBase)Activator.CreateInstance(
                    typeof(ValueMonitoredItem<>).MakeGenericType(propType),
                    this,
                     monitoredItemProperyInfoDescriptor.PropertyInfo,
                    ExpandedNodeId.Parse(mia.NodeId),
                    mia.AttributeId,
                    mia.IndexRange,
                    MonitoringMode.Reporting,
                    mia.SamplingInterval,
                    filter,
                    mia.QueueSize,
                    mia.DiscardOldest)!);

            }

            _stateMachineCts = new CancellationTokenSource();
            _stateMachineTask = Task.Run(() => StateMachineAsync(_stateMachineCts.Token));
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="CommunicationState"/>.
        /// </summary>
        public CommunicationState State
        {
            get => _state;
            private set => SetProperty(ref _state, value);
        }

        /// <summary>
        /// Gets the current subscription Id.
        /// </summary>
        public uint SubscriptionId => _state == CommunicationState.Opened ? _subscriptionId : 0u;
        #endregion Public Properties

        #region Protected Properties
        /// <summary>
        /// Gets the inner channel.
        /// </summary>
        protected ClientSessionChannel InnerChannel => _innerChannel ?? throw new ServiceResultException(StatusCodes.BadServerNotConnected);
        #endregion Protected Properties

        #region Public methods
        /// <summary>
        /// Sets an error to current <see cref="SubscriptionBase"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property that has errors.</param>
        /// <param name="errors">An <see cref="IEnumerable{T}"/> of string that contains error.</param>
        public void SetErrors(string propertyName, IEnumerable<string>? errors)
        {
            if (!_errors.TryGetValue(propertyName, out var propertyErrors))
            {
                propertyErrors = new List<string>();
                _errors.Add(propertyName, propertyErrors);
            }

            if (propertyErrors.Count > 0)
            {
                propertyErrors.Clear();
            }

            if (errors?.Count() > 0)
            {
                propertyErrors.AddRange(errors);
            }
        }

        /// <summary>
        /// Requests a Refresh of all Conditions.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<StatusCode> ConditionRefreshAsync()
        {
            if (State != CommunicationState.Opened)
            {
                return StatusCodes.BadServerNotConnected;
            }

            return await InnerChannel.ConditionRefreshAsync(SubscriptionId);
        }

        /// <summary>
        /// Acknowledges a condition.
        /// </summary>
        /// <param name="condition">an AcknowledgeableCondition.</param>
        /// <param name="comment">a comment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<StatusCode> AcknowledgeAsync(AcknowledgeableCondition condition, LocalizedText? comment = null)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (State != CommunicationState.Opened)
            {
                return StatusCodes.BadServerNotConnected;
            }

            return await InnerChannel.AcknowledgeAsync(condition, comment);
        }

        /// <summary>
        /// Confirms a condition.
        /// </summary>
        /// <param name="condition">an AcknowledgeableCondition.</param>
        /// <param name="comment">a comment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<StatusCode> ConfirmAsync(AcknowledgeableCondition condition, LocalizedText? comment = null)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (State != CommunicationState.Opened)
            {
                return StatusCodes.BadServerNotConnected;
            }

            return await InnerChannel.ConfirmAsync(condition, comment);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources asynchronously.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            var result = DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);

            return result;
        }
        #endregion Public methods

        #region Protected methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        protected async virtual ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && State == CommunicationState.Opened)
                {
                    try
                    {
                        await InnerChannel.DeleteSubscriptionsAsync(new DeleteSubscriptionsRequest() { SubscriptionIds = [SubscriptionId] });
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                _disposed = true;
            }
        }
        #endregion Protected methods

        #region Internal methods
        internal ValidationResult? ValidateProperty(string? propertyName, ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return ValidationResult.Success;
            }

            if (_errors.TryGetValue(propertyName, out var propertyErrors))
            {
                return new ValidationResult(propertyErrors.FirstOrDefault());
            }

            return ValidationResult.Success;
        }
        #endregion Internal methods

        #region Private Methods
        /// <summary>
        /// Handle PublishResponse message.
        /// </summary>
        /// <param name="publishResponse">The publish response.</param>
        private void OnPublishResponse(PublishResponse publishResponse)
        {
            _isPublishing = true;
            try
            {
                // loop thru all the notifications
                var nd = publishResponse.NotificationMessage?.NotificationData;
                if (nd == null)
                {
                    return;
                }

                foreach (var n in nd)
                {
                    // if data change.
                    var dcn = n as DataChangeNotification;
                    if (dcn?.MonitoredItems != null)
                    {
                        foreach (var min in dcn.MonitoredItems)
                        {
                            if (min?.Value == null)
                            {
                                _logger?.LogError($"One of the monitored item notifications is null");
                                continue;
                            }

                            if (_monitoredItems.TryGetValueByClientId(min.ClientHandle, out var item))
                            {
                                try
                                {
                                    item.Publish(min.Value);
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError($"Error publishing value for NodeId {item.NodeId}. {ex.Message}");
                                }
                            }
                        }

                        continue;
                    }

                    // if event.
                    var enl = n as EventNotificationList;
                    if (enl?.Events != null)
                    {
                        foreach (var efl in enl.Events)
                        {
                            if (efl?.EventFields == null)
                            {
                                _logger?.LogError($"One of the event field list is null");
                                continue;
                            }

                            if (_monitoredItems.TryGetValueByClientId(efl.ClientHandle, out var item))
                            {
                                try
                                {
                                    item.Publish(efl.EventFields);
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError($"Error publishing event for NodeId {item.NodeId}. {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                _isPublishing = false;
            }
        }

        /// <summary>
        /// Handles PropertyChanged event. If the property is associated with a MonitoredItem, writes the property value to the node of the server.
        /// </summary>
        /// <param name="sender">the sender.</param>
        /// <param name="e">the event.</param>
        private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (_isPublishing || string.IsNullOrEmpty(e.PropertyName))
            {
                return;
            }


            if (_monitoredItems.TryGetValueByName(e.PropertyName, out var item))
            {
                StatusCode statusCode = StatusCodes.Good;

                Type? serverType = null;

                try
                {
                    var readRequest = new ReadRequest()
                    {
                        NodesToRead = [new ReadValueId() { AttributeId = AttributeIds.Value,
                        NodeId = ExpandedNodeId.ToNodeId(item.NodeId, InnerChannel.NamespaceUris) }],
                        TimestampsToReturn = TimestampsToReturn.Neither
                    };

                    var readResponse = await InnerChannel.ReadAsync(readRequest);

                    serverType = readResponse.Results?.FirstOrDefault()?.Value?.GetType();
                }
                catch (ServiceResultException ex)
                {
                    statusCode = ex.StatusCode;
                }
                catch (Exception)
                {
                    statusCode = StatusCodes.BadServerNotConnected;
                }

                if (item is DataValueMonitoredItem dataValueMonitoredItem && dataValueMonitoredItem.TryGetValue(out var value, serverType) && dataValueMonitoredItem.IsValid())
                {
                    try
                    {
                        var writeRequest = new WriteRequest
                        {
                            NodesToWrite = [new WriteValue { NodeId = ExpandedNodeId.ToNodeId(item.NodeId, InnerChannel.NamespaceUris), AttributeId = item.AttributeId, IndexRange = item.IndexRange, Value = value }]
                        };
                        var writeResponse = await InnerChannel.WriteAsync(writeRequest).ConfigureAwait(false);
                        statusCode = writeResponse?.Results?[0] ?? StatusCodes.BadDataEncodingInvalid;
                    }
                    catch (ServiceResultException ex)
                    {
                        statusCode = ex.StatusCode;
                    }
                    catch (Exception)
                    {
                        statusCode = StatusCodes.BadServerNotConnected;
                    }
                }

                item.OnWriteResult(statusCode);
                if (StatusCode.IsBad(statusCode))
                {
                    _logger?.LogError($"Error writing value for {item.NodeId}. {StatusCodes.GetDefaultMessage(statusCode)}");
                }
            }
        }

        /// <summary>
        /// Signals the channel state is Closing.
        /// </summary>
        /// <param name="channel">The session channel. </param>
        /// <param name="token">A cancellation token. </param>
        /// <returns>A task.</returns>
        private async Task WhenChannelClosingAsync(ClientSessionChannel channel, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            EventHandler handler = (o, e) => tcs.TrySetResult(true);
            using (token.Register(state => ((TaskCompletionSource<bool>)state!).TrySetCanceled(), tcs, false))
            {
                try
                {
                    channel.Closing += handler;
                    if (channel.State == CommunicationState.Opened)
                    {
                        await tcs.Task;
                    }
                }
                finally
                {
                    channel.Closing -= handler;
                }
            }
        }

        /// <summary>
        /// The state machine manages the state of the subscription.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task.</returns>
        private async Task StateMachineAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //  await this.whenSubscribed.Task;

                _progress.Report(CommunicationState.Opening);

                try
                {
                    if (_endpointUrl is null)
                    {
                        throw new InvalidOperationException("The endpointUrl field must not be null. Please, use the Subscription attribute properly.");
                    }

                    // get a channel.
                    _innerChannel = await _application.GetChannelAsync(_endpointUrl, cancellationToken);

                    try
                    {
                        // create the subscription.
                        var subscriptionRequest = new CreateSubscriptionRequest
                        {
                            RequestedPublishingInterval = _publishingInterval,
                            RequestedMaxKeepAliveCount = _keepAliveCount,
                            RequestedLifetimeCount = Math.Max(_lifetimeCount, 3 * _keepAliveCount),
                            PublishingEnabled = true
                        };
                        var subscriptionResponse = await _innerChannel.CreateSubscriptionAsync(subscriptionRequest, cancellationToken).ConfigureAwait(false);

                        // link up the dataflow blocks
                        var id = _subscriptionId = subscriptionResponse.SubscriptionId;
                        var linkToken = _innerChannel.LinkTo(_actionBlock, pr => pr.SubscriptionId == id);

                        try
                        {
                            // create the monitored items.
                            var items = _monitoredItems.ToList();
                            if (items.Count > 0)
                            {
                                var requests = items.Select(m => new MonitoredItemCreateRequest { ItemToMonitor = new ReadValueId { NodeId = ExpandedNodeId.ToNodeId(m.NodeId, InnerChannel.NamespaceUris), AttributeId = m.AttributeId, IndexRange = m.IndexRange }, MonitoringMode = m.MonitoringMode, RequestedParameters = new MonitoringParameters { ClientHandle = m.ClientId, DiscardOldest = m.DiscardOldest, QueueSize = m.QueueSize, SamplingInterval = m.SamplingInterval, Filter = m.Filter } }).ToArray();

                                //split requests array to MaxMonitoredItemsPerCall chunks
                                var maxmonitoreditemspercall = 100;
                                MonitoredItemCreateRequest[] requests_chunk;
                                int chunk_size;
                                for (var i_chunk = 0; i_chunk < requests.Length; i_chunk += maxmonitoreditemspercall)
                                {
                                    chunk_size = Math.Min(maxmonitoreditemspercall, requests.Length - i_chunk);
                                    requests_chunk = new MonitoredItemCreateRequest[chunk_size];
                                    Array.Copy(requests, i_chunk, requests_chunk, 0, chunk_size);

                                    var itemsRequest = new CreateMonitoredItemsRequest
                                    {
                                        SubscriptionId = id,
                                        ItemsToCreate = requests_chunk,
                                    };
                                    var itemsResponse = await _innerChannel.CreateMonitoredItemsAsync(itemsRequest);

                                    if (itemsResponse.Results is { } results)
                                    {
                                        for (var i = 0; i < results.Length; i++)
                                        {
                                            var item = items[i];
                                            var result = results[i];

                                            if (result is null)
                                            {
                                                _logger?.LogError($"Error creating MonitoredItem for {item.NodeId}. The result is null.");
                                                continue;
                                            }

                                            item.OnCreateResult(result);
                                            if (StatusCode.IsBad(result.StatusCode))
                                            {
                                                _logger?.LogError($"Error creating MonitoredItem for {item.NodeId}. {StatusCodes.GetDefaultMessage(result.StatusCode)}");
                                            }
                                        }
                                    }
                                }
                            }

                            _progress.Report(CommunicationState.Opened);

                            // wait here until channel is closing, unsubscribed or token cancelled.
                            try
                            {
                                await WhenChannelClosingAsync(_innerChannel, cancellationToken);
                                //await Task.WhenAny(
                                //    this.WhenChannelClosingAsync(this.innerChannel, token),
                                //    this.whenUnsubscribed.Task);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                _progress.Report(CommunicationState.Closing);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError($"Error creating MonitoredItems. {ex.Message}");
                            _progress.Report(CommunicationState.Faulted);
                        }
                        finally
                        {
                            linkToken.Dispose();
                        }

                        if (_innerChannel.State == CommunicationState.Opened)
                        {
                            try
                            {
                                // delete the subscription.
                                var deleteRequest = new DeleteSubscriptionsRequest
                                {
                                    SubscriptionIds = [id]
                                };
                                await _innerChannel.DeleteSubscriptionsAsync(deleteRequest, cancellationToken);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError($"Error deleting subscription. {ex.Message}");
                                await Task.Delay(2000, cancellationToken);
                            }
                        }

                        _progress.Report(CommunicationState.Closed);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError($"Error creating subscription. {ex.Message}");
                        _progress.Report(CommunicationState.Faulted);
                        await Task.Delay(2000, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogTrace($"Error getting channel. {ex.Message}");
                    _progress.Report(CommunicationState.Faulted);
                    await Task.Delay(2000, cancellationToken);
                }
            }
        }

        private IReadOnlyList<MonitoredItemPropertyInfoDescriptor> GetMonitoredItemPropertyInfoDescriptors(Type type)
        {
            return _cachedMonitoredItemPropertyInfoDescriptors.GetOrAdd(type, t =>
            {
                var monitoredItemProperyInfoDescriptors = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 .Where(p => p.GetCustomAttribute<MonitoredItemAttribute>() != null)
                 .Select(pi => new MonitoredItemPropertyInfoDescriptor(pi, pi.GetCustomAttribute<MonitoredItemAttribute>()!)).ToList();

                var monitoredItemFieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(p => p.GetCustomAttribute<MonitoredItemAttribute>() != null)
                    .Select(fi => new { FieldInfo = fi, Attribute = fi.GetCustomAttribute<MonitoredItemAttribute>()! });

                if (monitoredItemFieldInfos.Any())
                {
                    var foundPropertyInfos = monitoredItemFieldInfos
                        .Where(fi => type.GetProperty(fi.FieldInfo.Name.ToPascalCase()) != null)
                        .Select(fi => new MonitoredItemPropertyInfoDescriptor(type.GetProperty(fi.FieldInfo.Name.ToPascalCase())!, fi.Attribute));

                    monitoredItemProperyInfoDescriptors.AddRange(foundPropertyInfos);
                }

                return monitoredItemProperyInfoDescriptors;
            });
        }
        #endregion Private Methods

        private class MonitoredItemPropertyInfoDescriptor
        {
            #region Constructors
            public MonitoredItemPropertyInfoDescriptor(PropertyInfo propertyInfo, MonitoredItemAttribute monitoredItemAttribute)
            {
                PropertyInfo = propertyInfo;
                MonitoredItemAttribute = monitoredItemAttribute;
            }
            #endregion Constructors

            #region Public properties
            public PropertyInfo PropertyInfo
            {
                get; set;
            }
            public MonitoredItemAttribute MonitoredItemAttribute
            {
                get; set;
            }
            #endregion Public properties
        }
    }
}
