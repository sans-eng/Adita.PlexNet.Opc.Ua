using Adita.PlexNet.Opc.Ua.Channels;

namespace Adita.PlexNet.Opc.Ua.Options;

/// <summary>
/// The <see cref="ClientSecureChannel"/> options.
/// </summary>
public class ClientSecureChannelOptions : ClientTransportChannelOptions
{
    /// <summary>
    /// Gets or sets the default number of milliseconds that may elapse before an operation is cancelled by the service.
    /// </summary>
    public uint TimeoutHint { get; set; } = ClientSecureChannel.DefaultTimeoutHint;

    /// <summary>
    /// Gets or sets the default diagnostics flags to be requested by the service.
    /// </summary>
    public uint DiagnosticsHint { get; set; } = ClientSecureChannel.DefaultDiagnosticsHint;
}
