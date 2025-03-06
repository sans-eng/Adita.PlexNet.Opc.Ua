// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A map between a requested endpoint url and the endpoint.
    /// </summary>
    public class MappedEndpoint
    {
        public string? RequestedUrl { get; set; }

        public EndpointDescription? Endpoint { get; set; }
    }
}
