// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;

namespace Adita.PlexNet.Opc.Ua.Abstractions.Encodables
{
    public interface IEncodable
    {
        void Encode(IEncoder encoder);

        void Decode(IDecoder decoder);
    }
}