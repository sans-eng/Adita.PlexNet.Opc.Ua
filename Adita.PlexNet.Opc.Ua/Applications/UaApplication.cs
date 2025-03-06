// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions;
using Adita.PlexNet.Opc.Ua.Abstractions.Identities;
using Adita.PlexNet.Opc.Ua.Channels;
using Adita.PlexNet.Opc.Ua.Extensions;
using Adita.PlexNet.Opc.Ua.Identities;
using Adita.PlexNet.Opc.Ua.Options;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Adita.PlexNet.Opc.Ua.Applications
{
    /// <summary>
    /// A <see cref="UaApplication"/>.
    /// </summary>
    public class UaApplication : IDisposable
    {
        private static readonly object globalLock = new object();
        private static volatile UaApplication? appInstance;

        private readonly ILogger? logger;
        private readonly ConcurrentDictionary<string, Lazy<Task<ClientSessionChannel>>> channelMap;
        private readonly TaskCompletionSource<bool> completionTask = new TaskCompletionSource<bool>();
        private volatile TaskCompletionSource<bool> suspensionTask = new TaskCompletionSource<bool>();
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UaApplication"/> class.
        /// </summary>
        /// <param name="localDescription">The <see cref="ApplicationDescription"/> of the local application.</param>
        /// <param name="certificateStore">The local certificate store.</param>
        /// <param name="identityProvider">An asynchronous function that provides the user identity. Provide an <see cref="AnonymousIdentity"/>, <see cref="UserNameIdentity"/>, <see cref="IssuedIdentity"/> or <see cref="X509Identity"/>.</param>
        /// <param name="mappedEndpoints">The mapped endpoints.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="options">The application options.</param>
        public UaApplication(
            ApplicationDescription localDescription,
            ICertificateStore? certificateStore,
            Func<EndpointDescription, Task<IUserIdentity>>? identityProvider,
            IEnumerable<MappedEndpoint> mappedEndpoints,
            ILoggerFactory? loggerFactory = null,
            UaApplicationOptions? options = null)
        {
            if (localDescription == null)
            {
                throw new ArgumentNullException(nameof(localDescription));
            }

            LocalDescription = localDescription;
            CertificateStore = certificateStore;
            UserIdentityProvider = identityProvider;
            MappedEndpoints = mappedEndpoints;
            LoggerFactory = loggerFactory;
            Options = options ?? new UaApplicationOptions();
            logger = loggerFactory?.CreateLogger<UaApplication>();
            channelMap = new ConcurrentDictionary<string, Lazy<Task<ClientSessionChannel>>>();

            lock (globalLock)
            {
                if (appInstance != null)
                {
                    throw new InvalidOperationException("You can only create a single instance of this type.");
                }

                appInstance = this;
            }
        }

        /// <summary>
        /// Gets the current <see cref="UaApplication"/>.
        /// </summary>
        public static UaApplication? Current => appInstance;

        /// <summary>
        /// Gets the <see cref="ApplicationDescription"/> of the local application.
        /// </summary>
        public ApplicationDescription LocalDescription { get; }

        /// <summary>
        /// Gets the local certificate store.
        /// </summary>
        public ICertificateStore? CertificateStore { get; }

        /// <summary>
        /// Gets an asynchronous function that provides the identity of the user. Supports <see cref="AnonymousIdentity"/>, <see cref="UserNameIdentity"/>, <see cref="IssuedIdentity"/> and <see cref="X509Identity"/>.
        /// </summary>
        public Func<EndpointDescription, Task<IUserIdentity>>? UserIdentityProvider { get; }

        /// <summary>
        /// Gets the mapped endpoints.
        /// </summary>
        public IEnumerable<MappedEndpoint> MappedEndpoints { get; }

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        public ILoggerFactory? LoggerFactory { get; }

        /// <summary>
        /// Gets the application options.
        /// </summary>
        public UaApplicationOptions Options { get; }

        /// <summary>
        /// Gets a System.Threading.Tasks.Task that represents the completion of the UaApplication.
        /// </summary>
        internal Task Completion => completionTask.Task;

        /// <summary>
        /// Closes the communication channels.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closes the communication channels to the remote endpoint.
        /// </summary>
        /// <param name="disposing">If true, then dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing & !disposed)
            {
                disposed = true;
                completionTask.TrySetResult(true);

                lock (globalLock)
                {
                    appInstance = null;
                }

                foreach (var value in channelMap.Values)
                {
                    var task = value.Value;
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        try
                        {
                            task.Result.CloseAsync().Wait(2000);
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// Suspends the communication channels to the remote endpoints.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that suspends the communication channel.</returns>
        public async Task SuspendAsync()
        {
            logger?.LogTrace($"UaApplication suspended.");
            if (suspensionTask.Task.IsCompleted)
            {
                suspensionTask = new TaskCompletionSource<bool>();
            }

            foreach (var value in channelMap.Values)
            {
                var task = value.Value;
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        await task.Result.CloseAsync().ConfigureAwait(false);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Creates the communication channels to the remote endpoints.
        /// </summary>
        public void Run()
        {
            logger?.LogTrace($"UaApplication running.");
            suspensionTask.TrySetResult(true);
        }

        /// <summary>
        /// Checks if application state is suspended.
        /// </summary>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task.</returns>
        private Task CheckSuspension(CancellationToken token = default)
        {
            return suspensionTask.Task.WithCancellation(token);
        }

        /// <summary>
        /// Gets or creates an <see cref="ClientSessionChannel"/>.
        /// </summary>
        /// <param name="endpointUrl">The endpoint url of the OPC UA server</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A <see cref="ClientSessionChannel"/>.</returns>
        public async Task<ClientSessionChannel> GetChannelAsync(string endpointUrl, CancellationToken token = default)
        {
            logger?.LogTrace($"Begin getting {nameof(ClientSessionChannel)} for {endpointUrl}");
            if (string.IsNullOrEmpty(endpointUrl))
            {
                throw new ArgumentNullException(nameof(endpointUrl));
            }

            await CheckSuspension(token).ConfigureAwait(false);

            var ch = await channelMap
                .GetOrAdd(endpointUrl, k => new Lazy<Task<ClientSessionChannel>>(() => Task.Run(() => CreateChannelAsync(k, token))))
                .Value
                .ConfigureAwait(false);

            return ch;
        }

        private async Task<ClientSessionChannel> CreateChannelAsync(string endpointUrl, CancellationToken token = default)
        {
            try
            {
                logger?.LogTrace($"Begin creating {nameof(ClientSessionChannel)} for {endpointUrl}");
                await CheckSuspension(token).ConfigureAwait(false);

                EndpointDescription endpoint;
                var mappedEndpoint = MappedEndpoints?.LastOrDefault(m => m.RequestedUrl == endpointUrl);
                if (mappedEndpoint?.Endpoint != null)
                {
                    endpoint = mappedEndpoint.Endpoint;
                }
                else
                {
                    endpoint = new EndpointDescription { EndpointUrl = endpointUrl };
                }

                var channel = new ClientSessionChannel(
                    LocalDescription,
                    CertificateStore,
                    UserIdentityProvider,
                    endpoint,
                    LoggerFactory,
                    Options);

                channel.Faulted += (s, e) =>
                {
                    logger?.LogTrace($"Error creating {nameof(ClientSessionChannel)} for {endpointUrl}. OnFaulted");
                    var ch = (ClientSessionChannel)s!;
                    try
                    {
                        ch.AbortAsync().Wait();
                    }
                    catch
                    {
                    }
                };

                channel.Closing += (s, e) =>
                {
                    logger?.LogTrace($"Removing {nameof(ClientSessionChannel)} for {endpointUrl} from channelMap.");
                    channelMap.TryRemove(endpointUrl, out _);
                };

                await channel.OpenAsync(token).ConfigureAwait(false);
                logger?.LogTrace($"Success creating {nameof(ClientSessionChannel)} for {endpointUrl}.");
                return channel;

            }
            catch (Exception ex)
            {
                logger?.LogTrace($"Error creating {nameof(ClientSessionChannel)} for {endpointUrl}. {ex.Message}");
                throw;
            }
        }
    }
}
