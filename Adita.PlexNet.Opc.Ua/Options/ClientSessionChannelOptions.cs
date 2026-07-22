using Adita.PlexNet.Opc.Ua.Channels;

namespace Adita.PlexNet.Opc.Ua.Options;

/// <summary>
/// The <see cref="ClientSessionChannel"/> options.
/// </summary>
public class ClientSessionChannelOptions : ClientSecureChannelOptions
{
    /// <summary>
    /// Gets the requested number of milliseconds that a session may be unused before being closed by the server.
    /// </summary>
    public double SessionTimeout { get; set; } = ClientSessionChannel.DefaultSessionTimeout;
}
