// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua.Annotations
{
    /// <summary>
    /// Attribute for classes of type IEncodable to indicate the binary encoding id.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class BinaryEncodingIdAttribute : Attribute
    {
        public BinaryEncodingIdAttribute(string s)
        {
            NodeId = ExpandedNodeId.Parse(s);
        }

        public ExpandedNodeId NodeId { get; }
    }
}