// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Channels;

namespace Adita.PlexNet.Opc.Ua.Extensions
{
    /// <summary>
    /// Represents a method service set extensions.
    /// </summary>
    public static class MethodServiceSetExtensions
    {
        /// <summary>
        /// Calls (invokes) a list of Methods.
        /// </summary>
        /// <param name="channel">A instance of <see cref="IRequestChannel"/>.</param>
        /// <param name="request">A <see cref="CallRequest"/>.</param>
        /// <param name="token">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="CallResponse"/>.</returns>
        /// <seealso href="https://reference.opcfoundation.org/v104/Core/docs/Part4/5.11.2/">OPC UA specification Part 4: Services, 5.11.2</seealso>
        public static async Task<CallResponse> CallAsync(this IRequestChannel channel, CallRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return (CallResponse)await channel.RequestAsync(request, token).ConfigureAwait(false);
        }

    }
}
