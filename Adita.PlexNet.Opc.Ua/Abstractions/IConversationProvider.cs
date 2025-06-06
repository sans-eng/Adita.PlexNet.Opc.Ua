﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Options;
using Microsoft.Extensions.Logging;

namespace Adita.PlexNet.Opc.Ua.Abstractions
{
    /// <summary>
    /// Provider interface for <see cref="IConversation"/> instances.
    /// </summary>
    public interface IConversationProvider
    {
        /// <summary>
        /// Creates an <see cref="IConversation"/> instance for a client
        /// channel.
        /// </summary>
        /// <param name="remoteEndpoint">The remote endpoint.</param>
        /// <param name="localDescription">The local application description.</param>
        /// <param name="options">The transport connection options.</param>
        /// <param name="certificateStore">The certificate store.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>An <see cref="IConversation"/> instance.</returns>
        Task<IConversation> CreateAsync(EndpointDescription remoteEndpoint, ApplicationDescription localDescription, TransportConnectionOptions options, ICertificateStore? certificateStore, ILogger? logger, CancellationToken token);
    }
}
