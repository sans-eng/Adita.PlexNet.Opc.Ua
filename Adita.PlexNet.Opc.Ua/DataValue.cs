﻿// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Annotations;
using System.Text.Json.Serialization;

namespace Adita.PlexNet.Opc.Ua
{
    [DataTypeId(DataTypeIds.DataValue)]
    public sealed class DataValue
    {
        public DataValue(object? value, StatusCode statusCode = default, DateTime sourceTimestamp = default, ushort sourcePicoseconds = 0, DateTime serverTimestamp = default, ushort serverPicoseconds = 0)
        {
            WrappedValue = new Variant(value);
            StatusCode = statusCode;
            SourceTimestamp = sourceTimestamp;
            SourcePicoseconds = sourcePicoseconds;
            ServerTimestamp = serverTimestamp;
            ServerPicoseconds = serverPicoseconds;
        }
        [JsonConstructor]
        public DataValue(Variant wrappedValue)
        {
            WrappedValue = wrappedValue;
        }
        public DataValue(Variant wrappedValue, StatusCode statusCode = default, DateTime sourceTimestamp = default, ushort sourcePicoseconds = 0, DateTime serverTimestamp = default, ushort serverPicoseconds = 0)
        {
            WrappedValue = wrappedValue;
            StatusCode = statusCode;
            SourceTimestamp = sourceTimestamp;
            SourcePicoseconds = sourcePicoseconds;
            ServerTimestamp = serverTimestamp;
            ServerPicoseconds = serverPicoseconds;
        }
        [JsonIgnore]
        public object? Value
        {
            get { return WrappedValue.Value; }
        }
        [JsonIgnore]

        public StatusCode StatusCode { get; }
        [JsonIgnore]
        public DateTime SourceTimestamp { get; }
        [JsonIgnore]
        public ushort SourcePicoseconds { get; }
        [JsonIgnore]
        public DateTime ServerTimestamp { get; }
        [JsonIgnore]
        public ushort ServerPicoseconds { get; }

        public Variant WrappedValue { get; }

        public override string ToString()
        {
            return $"{Value}; status: {StatusCode}; ts: {SourceTimestamp}";
        }
    }
}