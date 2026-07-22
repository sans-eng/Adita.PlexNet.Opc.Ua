// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Channels;

namespace Adita.PlexNet.Opc.Ua.Options;

/// <summary>
/// The UaApplication options.
/// </summary>
public class UaApplicationOptions
{
    #region Public Properties
    /// <summary>
    /// Gets or sets <see cref="ServerCapabilitiesOptions"/> of current <see cref="UaApplicationOptions"/>.
    /// </summary>
    public ServerCapabilitiesOptions ServerCapabilities
    {
        get;
        set;
    } = new();
    /// <summary>
    /// Gets or sets <see cref="OperationLimitsOptions"/> of current <see cref="UaApplicationOptions"/>.
    /// </summary>
    public OperationLimitsOptions OperationLimits
    {
        get;
        set;
    } = new();
    /// <summary>
    /// Gets or sets <see cref="ClientSessionChannelOptions"/> of current <see cref="UaApplicationOptions"/>.
    /// </summary>
    public ClientSessionChannelOptions ClientSessionChannel
    {
        get;
        set;
    } = new();
    #endregion Public Properties
}
