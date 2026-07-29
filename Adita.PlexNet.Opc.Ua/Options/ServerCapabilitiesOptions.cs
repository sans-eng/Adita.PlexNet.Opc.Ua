using System.Security.Cryptography.X509Certificates;

namespace Adita.PlexNet.Opc.Ua.Options;

/// <summary>
/// Represents the server capabilities and constraints for an OPC UA server.
/// </summary>
public class ServerCapabilitiesOptions
{
    #region Public properties
    /// <summary>
    /// Gets or sets the server profile array.
    /// </summary>
    public string[] ServerProfileArray
    {
        get;
        set;
    } = [];

    /// <summary>
    /// Gets or sets the locale ID array.
    /// </summary>
    public string[] LocaleIdArray
    {
        get;
        set;
    } = [];

    /// <summary>
    /// Gets or sets the minimum supported sample rate. Default is 100.
    /// </summary>
    public uint MinSupportedSampleRate
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the maximum browse continuation points. Default is 100.
    /// </summary>
    public ushort MaxBrowseContinuationPoints
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the maximum query continuation points. Default is 100.
    /// </summary>
    public ushort MaxQueryContinuationPoints
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the maximum history continuation points. Default is 100.
    /// </summary>
    public ushort MaxHistoryContinuationPoints
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the software certificates array.
    /// </summary>
    public X509Certificate[] SoftwareCertificates
    {
        get;
        set;
    } = [];

    /// <summary>
    /// Gets or sets the maximum array length. Default is 65536.
    /// </summary>
    public uint MaxArrayLength
    {
        get;
        set;
    } = 65536;

    /// <summary>
    /// Gets or sets the maximum string length. Default is 65536.
    /// </summary>
    public uint MaxStringLength
    {
        get;
        set;
    } = 65536;

    /// <summary>
    /// Gets or sets the maximum byte string length. Default is 65536.
    /// </summary>
    public uint MaxByteStringLength
    {
        get;
        set;
    } = 65536;

    /// <summary>
    /// Gets or sets the maximum number of sessions. Default is 1.
    /// </summary>
    public uint MaxSessions
    {
        get;
        set;
    } = 1;

    /// <summary>
    /// Gets or sets the maximum number of subscriptions. Default is 15.
    /// </summary>
    public uint MaxSubscriptions
    {
        get;
        set;
    } = 15;

    /// <summary>
    /// Gets or sets the maximum number of monitored items. Default is 10000.
    /// </summary>
    public uint MaxMonitoredItems
    {
        get;
        set;
    } = 10000;

    /// <summary>
    /// Gets or sets the maximum subscriptions per session. Default is 15.
    /// </summary>
    public uint MaxSubscriptionsPerSession
    {
        get;
        set;
    } = 15;

    /// <summary>
    /// Gets or sets the maximum monitored items per subscription. Default is 2000.
    /// </summary>
    public uint MaxMonitoredItemsPerSubscription
    {
        get;
        set;
    } = 2000;

    /// <summary>
    /// Gets or sets the maximum select clause parameters. Default is 100.
    /// </summary>
    public uint MaxSelectClauseParameters
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the maximum where clause parameters. Default is 100.
    /// </summary>
    public uint MaxWhereClauseParameters
    {
        get;
        set;
    } = 100;

    /// <summary>
    /// Gets or sets the maximum monitored item queue size. Default is 100.
    /// </summary>
    public uint MaxMonitoredItemQueueSize
    {
        get;
        set;
    } = 100;
    #endregion Public properties
}
