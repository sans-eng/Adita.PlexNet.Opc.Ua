// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Adita.PlexNet.Opc.Ua.Annotations
{
    /// <summary>
    /// Specifies an assembly that provides custom types for the encoders.
    /// <para>
    /// The assembly is searched for types with <see cref="BinaryEncodingIdAttribute" />.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class TypeLibraryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationPartAttribute" />.
        /// </summary>
        public TypeLibraryAttribute()
        {
        }
    }
}
