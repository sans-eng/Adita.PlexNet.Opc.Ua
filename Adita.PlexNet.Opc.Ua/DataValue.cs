// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Annotations;
using System.Text.Json.Serialization;

namespace Adita.PlexNet.Opc.Ua
{
    [DataTypeId(DataTypeIds.DataValue)]
    public sealed class DataValue
    {
        [JsonConstructor]
        public DataValue(object? value, StatusCode statusCode = default, DateTime sourceTimestamp = default, ushort sourcePicoseconds = 0, DateTime serverTimestamp = default, ushort serverPicoseconds = 0)
        {
            Variant = new Variant(value);
            StatusCode = statusCode;
            SourceTimestamp = sourceTimestamp;
            SourcePicoseconds = sourcePicoseconds;
            ServerTimestamp = serverTimestamp;
            ServerPicoseconds = serverPicoseconds;
        }
        public DataValue(Variant variant, StatusCode statusCode = default, DateTime sourceTimestamp = default, ushort sourcePicoseconds = 0, DateTime serverTimestamp = default, ushort serverPicoseconds = 0)
        {
            Variant = variant;
            StatusCode = statusCode;
            SourceTimestamp = sourceTimestamp;
            SourcePicoseconds = sourcePicoseconds;
            ServerTimestamp = serverTimestamp;
            ServerPicoseconds = serverPicoseconds;
        }

        public object? Value
        {
            get { return Variant.Value; }
        }

        public StatusCode StatusCode { get; }

        public DateTime SourceTimestamp { get; }

        public ushort SourcePicoseconds { get; }

        public DateTime ServerTimestamp { get; }

        public ushort ServerPicoseconds { get; }

        public Variant Variant { get; }

        public override string ToString()
        {
            return $"{Value}; status: {StatusCode}; ts: {SourceTimestamp}";
        }
    }
}