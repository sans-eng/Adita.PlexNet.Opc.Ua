// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Adita.PlexNet.Opc.Ua.Channels
{
    public interface IEncodingContext
    {
        IReadOnlyList<string> NamespaceUris { get; }
        IReadOnlyList<string> ServerUris { get; }
        int MaxStringLength { get; }
        int MaxArrayLength { get; }
        int MaxByteStringLength { get; }
    }

    public class DefaultEncodingContext : IEncodingContext
    {
        public IReadOnlyList<string> NamespaceUris => new List<string> { "http://opcfoundation.org/UA/" };

        public IReadOnlyList<string> ServerUris => new List<string>();

        public int MaxStringLength => 65535;

        public int MaxArrayLength => 65535;

        public int MaxByteStringLength => 65535;

    }

}
