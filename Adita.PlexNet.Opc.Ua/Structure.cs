// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>
    /// A base implementation of a Structure.
    /// </summary>
    [DataTypeId(DataTypeIds.Structure)]
    public abstract class Structure : ObservableValidator, IEncodable
    {
        public virtual bool IsDefault
        {
            get;
        }

        public abstract void Encode(IEncoder encoder);

        public abstract void Decode(IDecoder decoder);
    }
}
