// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Options;

namespace Adita.PlexNet.Opc.Ua.Abstractions
{
    /// <summary>
    /// Provider interface for <see cref="ITransportConnection"/> instances.
    /// </summary>
    public interface ITransportConnectionProvider
    {
        /// <summary>
        /// Creates a transport connection.
        /// </summary>
        /// <remarks>
        /// The implementation can already open the bare network connection, or
        /// defer this process to the <see cref="ITransportConnection.OpenAsync(uint, TransportConnectionOptions, CancellationToken)"/>
        /// method.
        /// </remarks>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The transport connection.</returns>
        Task<ITransportConnection> ConnectAsync(string connectionString, CancellationToken token);
    }
}
