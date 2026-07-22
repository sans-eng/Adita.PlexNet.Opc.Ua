using Adita.PlexNet.Opc.Ua.Applications;
using Adita.PlexNet.Opc.Ua.Options;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Opc.Ua;

public abstract class SubscriptionBatchedBase(UaApplication? uaApplication, IOptions<ServerCapabilitiesOptions>? serverCapabilitiesOptions = default, IOptions<OperationLimitsOptions>? operationLimitsOptions = default)
{
    #region Private fields
    private readonly UaApplication _uaApplication = uaApplication ?? throw new ArgumentNullException(nameof(uaApplication));
    private readonly ServerCapabilitiesOptions _serverCapabilitiesOptions = serverCapabilitiesOptions != null ? serverCapabilitiesOptions.Value : uaApplication.Options.ServerCapabilities;
    private readonly OperationLimitsOptions _operationLimitsOptions = operationLimitsOptions != null ? operationLimitsOptions.Value : uaApplication.Options.OperationLimits;
    #endregion Private fields

    #region Constructors
    protected SubscriptionBatchedBase()
        : this(UaApplication.Current)
    {
    }
    protected SubscriptionBatchedBase(IOptions<ServerCapabilitiesOptions> serverCapabilitiesOptions, IOptions<OperationLimitsOptions> operationLimitsOptions)
    : this(UaApplication.Current, serverCapabilitiesOptions, operationLimitsOptions)
    {
    }
    #endregion Constructors
}
