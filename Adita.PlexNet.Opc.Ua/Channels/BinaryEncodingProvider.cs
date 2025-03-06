// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions;
using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Decoders;
using Adita.PlexNet.Opc.Ua.Encoders;

namespace Adita.PlexNet.Opc.Ua.Channels
{
    /// <summary>
    /// The <see cref="IEncodingProvider"/> interface implementation
    /// for the OPC UA Binary DataEncoding.
    /// </summary>
    /// <seealso href="https://reference.opcfoundation.org/v105/Core/docs/Part6/5.2.1/">OPC UA specification Part 6: Mappings, 5.2.1</seealso>
    public class BinaryEncodingProvider : IEncodingProvider
    {
        /// <inheritdoc />
        public IDecoder CreateDecoder(Stream stream, IEncodingContext? context, bool keepStreamOpen)
        {
            return new BinaryDecoder(stream, context, keepStreamOpen);
        }

        /// <inheritdoc />
        public IEncoder CreateEncoder(Stream stream, IEncodingContext? context, bool keepStreamOpen)
        {
            return new BinaryEncoder(stream, context, keepStreamOpen);
        }
    }
}
