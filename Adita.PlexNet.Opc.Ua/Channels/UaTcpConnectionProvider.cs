﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions;
using Adita.PlexNet.Opc.Ua.Extensions;
using System.Net.Sockets;

namespace Adita.PlexNet.Opc.Ua.Channels
{
    /// <summary>
    /// The <see cref="ITransportConnectionProvider"/> interface implementation
    /// for the OPC UA TCP transport protocol.
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part6/7.2/">OPC UA specification Part 6: Mappings, 7.2</seealso>
    public class UaTcpConnectionProvider : ITransportConnectionProvider
    {
        private const int ConnectTimeout = 5000;

        /// <inheritdoc />
        public async Task<ITransportConnection> ConnectAsync(string connectionString, CancellationToken token)
        {
            var uri = new Uri(connectionString);
            var client = new TcpClient
            {
                NoDelay = true
            };

            await client.ConnectAsync(uri.Host, uri.Port).TimeoutAfter(ConnectTimeout, token).ConfigureAwait(false);

            // The stream will own the client and takes care on disposing/closing it
            return new UaClientConnection(client.GetStream(), uri);
        }
    }
}
