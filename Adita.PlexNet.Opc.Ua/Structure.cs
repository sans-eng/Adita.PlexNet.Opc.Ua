// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using Adita.PlexNet.Opc.Ua.Extensions;
using Adita.PlexNet.Opc.Ua.Internal.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A base implementation of a Structure.
    /// </summary>
    [DataTypeId(DataTypeIds.Structure)]
    public abstract class Structure : IEncodable
    {
        #region Public methods
        /// <summary>
        /// Encode current <see cref="Structure"/> using specified <paramref name="encoder"/>.
        /// </summary>
        /// <param name="encoder">The <see cref="IEncoder"/> to encode <see cref="Structure"/>.</param>
        public abstract void Encode(IEncoder encoder);
        /// <summary>
        /// Decode current <see cref="Structure"/> using specified <paramref name="decoder"/>.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> to decode <see cref="Structure"/>.</param>
        public abstract void Decode(IDecoder decoder);
        #endregion Public methods
    }
}
