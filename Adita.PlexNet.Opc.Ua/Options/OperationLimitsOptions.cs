namespace Adita.PlexNet.Opc.Ua.Options;

/// <summary>
/// Represents configuration options for OPC UA operation limits.
/// </summary>
public class OperationLimitsOptions
{
    #region Public properties
    /// <summary>
    /// Gets or sets the maximum number of nodes per read operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerRead { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per write operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerWrite { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per history read data operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerHistoryReadData { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per history read events operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerHistoryReadEvents { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per history update data operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerHistoryUpdateData { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per method call operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerMethodCall { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per browse operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerBrowse { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per register nodes operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerRegisterNodes { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per translate browse paths to node IDs operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerTranslateBrowsePathsToNodeIds { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of nodes per node management operation. Default is 1000.
    /// </summary>
    public uint MaxNodesPerNodeManagement { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum number of monitored items per call operation. Default is 100.
    /// </summary>
    public uint MaxMonitoredItemsPerCall { get; set; } = 100;

    #endregion Public properties
}
