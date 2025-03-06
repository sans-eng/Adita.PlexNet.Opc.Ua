// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;

namespace Adita.PlexNet.Opc.Ua.Abstractions.Responses
{
    public interface IServiceResponse : IEncodable
    {
        ResponseHeader? ResponseHeader { get; set; }
    }
}