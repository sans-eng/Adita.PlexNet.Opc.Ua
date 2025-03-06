// Copyright (c) Converter Systems LLC. All right// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


// Copyright (c) Converter Systems LLC. All right// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Channels;

namespace Adita.PlexNet.Opc.Ua.Extensions
{
    /// <summary>
    /// Represents an attribute service set extension.
    /// </summary>
    public static class AttributeServiceSetExtensions
    {
        /// <summary>
        /// Reads a list of Node attributes.
        /// </summary>
        /// <param name="client">A instance of <see cref="IRequestChannel"/>.</param>
        /// <param name="request">A <see cref="ReadRequest"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="ReadResponse"/>.</returns>
        /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part4/5.10.2/">OPC UA specification Part 4: Services, 5.10.2</seealso>
        public static async Task<ReadResponse> ReadAsync(this IRequestChannel client, ReadRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (ReadResponse)await client.RequestAsync(request, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes a list of Node attributes.
        /// </summary>
        /// <param name="client">A instance of <see cref="IRequestChannel"/>.</param>
        /// <param name="request">A <see cref="WriteRequest"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="WriteResponse"/>.</returns>
        /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part4/5.10.4/">OPC UA specification Part 4: Services, 5.10.4</seealso>
        public static async Task<WriteResponse> WriteAsync(this IRequestChannel client, WriteRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (WriteResponse)await client.RequestAsync(request, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads historical values or Events of one or more Nodes.
        /// </summary>
        /// <param name="client">A instance of <see cref="IRequestChannel"/>.</param>
        /// <param name="request">A <see cref="HistoryReadRequest"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="HistoryReadResponse"/>.</returns>
        /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part4/5.10.3/">OPC UA specification Part 4: Services, 5.10.3</seealso>
        public static async Task<HistoryReadResponse> HistoryReadAsync(this IRequestChannel client, HistoryReadRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (HistoryReadResponse)await client.RequestAsync(request, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates historical values or Events of one or more Nodes.
        /// </summary>
        /// <param name="client">A instance of <see cref="IRequestChannel"/>.</param>
        /// <param name="request">A <see cref="HistoryUpdateRequest"/>.</param>\
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="HistoryUpdateResponse"/>.</returns>
        /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part4/5.10.5/">OPC UA specification Part 4: Services, 5.10.5</seealso>
        public static async Task<HistoryUpdateResponse> HistoryUpdateAsync(this IRequestChannel client, HistoryUpdateRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (HistoryUpdateResponse)await client.RequestAsync(request, token).ConfigureAwait(false);
        }
    }
}
