using Adita.PlexNet.Opc.Ua.Channels;

namespace Adita.PlexNet.Opc.Ua.Options;


/// <summary>
/// The <see cref="ClientTransportChannel"/> options.
/// </summary>
public class ClientTransportChannelOptions
{
    /// <summary>
    /// Gets or sets the size of the receive buffer.
    /// </summary>
    public uint LocalReceiveBufferSize { get; set; } = ClientTransportChannel.DefaultBufferSize;

    /// <summary>
    /// Gets or sets the size of the send buffer.
    /// </summary>
    public uint LocalSendBufferSize { get; set; } = ClientTransportChannel.DefaultBufferSize;

    /// <summary>
    /// Gets or sets the maximum total size of a message.
    /// </summary>
    public uint LocalMaxMessageSize { get; set; } = ClientTransportChannel.DefaultMaxMessageSize;

    /// <summary>
    /// Gets or sets the maximum number of message chunks.
    /// </summary>
    public uint LocalMaxChunkCount { get; set; } = ClientTransportChannel.DefaultMaxChunkCount;
}
