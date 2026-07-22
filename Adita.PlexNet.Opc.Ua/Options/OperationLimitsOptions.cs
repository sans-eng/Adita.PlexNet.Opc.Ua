namespace Adita.PlexNet.Opc.Ua.Options;

public class OperationLimitsOptions
{
    #region Public properties
    /// <summary>
    /// Gets or sets the maximum number of nodes per read operation.
    /// </summary>
    public uint MaxNodesPerRead
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per write operation.
    /// </summary>
    public uint MaxNodesPerWrite
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per history read data operation.
    /// </summary>
    public uint MaxNodesPerHistoryReadData
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per history read events operation.
    /// </summary>
    public uint MaxNodesPerHistoryReadEvents
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per history update data operation.
    /// </summary>
    public uint MaxNodesPerHistoryUpdateData
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per method call operation.
    /// </summary>
    public uint MaxNodesPerMethodCall
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per browse operation.
    /// </summary>
    public uint MaxNodesPerBrowse
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per register nodes operation.
    /// </summary>
    public uint MaxNodesPerRegisterNodes
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per translate browse paths to node IDs operation.
    /// </summary>
    public uint MaxNodesPerTranslateBrowsePathsToNodeIds
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of nodes per node management operation.
    /// </summary>
    public uint MaxNodesPerNodeManagement
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum number of monitored items per call operation.
    /// </summary>
    public uint MaxMonitoredItemsPerCall
    {
        get; set;
    }

    #endregion Public properties
}
