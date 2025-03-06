// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions;
using Adita.PlexNet.Opc.Ua.Options;
using Microsoft.Extensions.Logging;

namespace Adita.PlexNet.Opc.Ua.Channels
{
    /// <summary>
    /// The <see cref="IConversationProvider"/> interface implementation
    /// for the OPC UA Secure Conversation (UASC).
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part6/6.7.1/">OPC UA specification Part 6: Mappings, 7.2</seealso>
    public class UaSecureConversationProvider : IConversationProvider
    {
        /// <inheritdoc />
        public async Task<IConversation> CreateAsync(EndpointDescription remoteEndpoint, ApplicationDescription localDescription, TransportConnectionOptions options, ICertificateStore? certificateStore, ILogger? logger, CancellationToken token)
        {
            var conversation = new UaSecureConversation(localDescription, options, certificateStore, logger)
            {
                SecurityMode = remoteEndpoint.SecurityMode
            };

            await conversation.SetRemoteCertificateAsync(remoteEndpoint.SecurityPolicyUri, remoteEndpoint.ServerCertificate, token).ConfigureAwait(false);

            return conversation;
        }
    }
}
