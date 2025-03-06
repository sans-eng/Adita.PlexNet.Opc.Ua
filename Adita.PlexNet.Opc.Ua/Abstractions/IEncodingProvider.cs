// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Channels;

namespace Adita.PlexNet.Opc.Ua.Abstractions
{
    /// <summary>
    /// Provider interface for <see cref="IEncoder"/> and <see cref="IDecoder" />
    /// instances.
    /// </summary>
    public interface IEncodingProvider
    {
        /// <summary>
        /// Creates an encoder instance.
        /// </summary>
        /// <param name="stream">The target stream.</param>
        /// <param name="context">The encoding context.</param>
        /// <param name="keepStreamOpen">Whether the stream should remain open on disposal.</param>
        /// <returns>The encoder instance.</returns>
        IEncoder CreateEncoder(Stream stream, IEncodingContext? context, bool keepStreamOpen);

        /// <summary>
        /// Creates a decoder instance.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="context">The encoding context.</param>
        /// <param name="keepStreamOpen">Whether the stream should remain open on disposal.</param>
        /// <returns>The decoder instance.</returns>
        IDecoder CreateDecoder(Stream stream, IEncodingContext? context, bool keepStreamOpen);
    }
}
