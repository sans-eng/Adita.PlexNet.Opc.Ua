// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua.Annotations
{
    /// <summary>
    /// Attribute for classes to indicate the data type id.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class DataTypeIdAttribute : Attribute
    {
        public DataTypeIdAttribute(string s)
        {
            NodeId = ExpandedNodeId.Parse(s);
        }

        public ExpandedNodeId NodeId { get; }
    }
}