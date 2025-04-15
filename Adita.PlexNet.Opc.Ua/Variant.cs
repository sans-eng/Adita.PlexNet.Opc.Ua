// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Annotations;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace Adita.PlexNet.Opc.Ua
{
    public enum VariantType
    {
        Null = 0,
        Boolean = 1,
        SByte = 2,
        Byte = 3,
        Int16 = 4,
        UInt16 = 5,
        Int32 = 6,
        UInt32 = 7,
        Int64 = 8,
        UInt64 = 9,
        Float = 10,
        Double = 11,
        String = 12,
        DateTime = 13,
        Guid = 14,
        ByteString = 15,
        XmlElement = 16,
        NodeId = 17,
        ExpandedNodeId = 18,
        StatusCode = 19,
        QualifiedName = 20,
        LocalizedText = 21,
        ExtensionObject = 22,
        DataValue = 23,
        Variant = 24,
        DiagnosticInfo = 25,
    }

    [DataTypeId(DataTypeIds.BaseDataType)]
    public readonly struct Variant
    {
        public static readonly Variant Null = default;

        private static readonly Dictionary<Type, VariantType> typeMap = new Dictionary<Type, VariantType>()
        {
            [typeof(bool)] = VariantType.Boolean,
            [typeof(sbyte)] = VariantType.SByte,
            [typeof(byte)] = VariantType.Byte,
            [typeof(short)] = VariantType.Int16,
            [typeof(ushort)] = VariantType.UInt16,
            [typeof(int)] = VariantType.Int32,
            [typeof(uint)] = VariantType.UInt32,
            [typeof(long)] = VariantType.Int64,
            [typeof(ulong)] = VariantType.UInt64,
            [typeof(float)] = VariantType.Float,
            [typeof(double)] = VariantType.Double,
            [typeof(string)] = VariantType.String,
            [typeof(DateTime)] = VariantType.DateTime,
            [typeof(Guid)] = VariantType.Guid,
            [typeof(byte[])] = VariantType.ByteString,
            [typeof(XElement)] = VariantType.XmlElement,
            [typeof(NodeId)] = VariantType.NodeId,
            [typeof(ExpandedNodeId)] = VariantType.ExpandedNodeId,
            [typeof(StatusCode)] = VariantType.StatusCode,
            [typeof(QualifiedName)] = VariantType.QualifiedName,
            [typeof(LocalizedText)] = VariantType.LocalizedText,
            [typeof(ExtensionObject)] = VariantType.ExtensionObject,
            /*
            [typeof(DataValue)] = VariantType.DataValue,
            [typeof(Variant)] = VariantType.Variant,
            [typeof(DiagnosticInfo)] = VariantType.DiagnosticInfo,
            */
        };

        private static readonly Dictionary<Type, VariantType> elemTypeMap = new Dictionary<Type, VariantType>()
        {
            [typeof(bool)] = VariantType.Boolean,
            [typeof(sbyte)] = VariantType.SByte,
            [typeof(byte)] = VariantType.Byte,
            [typeof(short)] = VariantType.Int16,
            [typeof(ushort)] = VariantType.UInt16,
            [typeof(int)] = VariantType.Int32,
            [typeof(uint)] = VariantType.UInt32,
            [typeof(long)] = VariantType.Int64,
            [typeof(ulong)] = VariantType.UInt64,
            [typeof(float)] = VariantType.Float,
            [typeof(double)] = VariantType.Double,
            [typeof(string)] = VariantType.String,
            [typeof(DateTime)] = VariantType.DateTime,
            [typeof(Guid)] = VariantType.Guid,
            [typeof(byte[])] = VariantType.ByteString,
            [typeof(XElement)] = VariantType.XmlElement,
            [typeof(NodeId)] = VariantType.NodeId,
            [typeof(ExpandedNodeId)] = VariantType.ExpandedNodeId,
            [typeof(StatusCode)] = VariantType.StatusCode,
            [typeof(QualifiedName)] = VariantType.QualifiedName,
            [typeof(LocalizedText)] = VariantType.LocalizedText,
            [typeof(ExtensionObject)] = VariantType.ExtensionObject,
            [typeof(Variant)] = VariantType.Variant,
            /*
            [typeof(DataValue)] = VariantType.DataValue,
            [typeof(DiagnosticInfo)] = VariantType.DiagnosticInfo,
            */
        };

        public Variant(object? value)
        {
            if (value == null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Type type = value.GetType();
            VariantType variantType;
            if (typeMap.TryGetValue(type, out variantType))
            {
                Value = value;
                Type = variantType;
                ArrayDimensions = null;
                return;
            }

            var encodable = value as IEncodable;
            if (encodable != null)
            {
                Value = new ExtensionObject(encodable);
                Type = VariantType.ExtensionObject;
                ArrayDimensions = null;
                return;
            }

            if (value is Array array)
            {
                Type? elemType = array.GetType().GetElementType();
                if (elemType == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), elemType, "Array element Type is unsupported.");
                }

                if (elemTypeMap.TryGetValue(elemType, out variantType))
                {
                    Value = array;
                    Type = variantType;
                    ArrayDimensions = new int[array.Rank];
                    for (int i = 0; i < array.Rank; i++)
                    {
                        ArrayDimensions[i] = array.GetLength(i);
                    }

                    return;
                }

                if (elemType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEncodable)))
                {
                    Value = array.Cast<IEncodable>().Select(v => new ExtensionObject(v)).ToArray();
                    Type = VariantType.ExtensionObject;
                    ArrayDimensions = new int[array.Rank];
                    for (int i = 0; i < array.Rank; i++)
                    {
                        ArrayDimensions[i] = array.GetLength(i);
                    }

                    return;
                }

                throw new ArgumentOutOfRangeException(nameof(value), elemType, "Array element Type is unsupported.");
            }

            throw new ArgumentOutOfRangeException(nameof(value), type, "Type is unsupported.");
        }

        public Variant(bool value)
        {
            Value = value;
            Type = VariantType.Boolean;
            ArrayDimensions = null;
        }

        public Variant(sbyte value)
        {
            Value = value;
            Type = VariantType.SByte;
            ArrayDimensions = null;
        }

        public Variant(byte value)
        {
            Value = value;
            Type = VariantType.Byte;
            ArrayDimensions = null;
        }

        public Variant(short value)
        {
            Value = value;
            Type = VariantType.Int16;
            ArrayDimensions = null;
        }

        public Variant(ushort value)
        {
            Value = value;
            Type = VariantType.UInt16;
            ArrayDimensions = null;
        }

        public Variant(int value)
        {
            Value = value;
            Type = VariantType.Int32;
            ArrayDimensions = null;
        }

        public Variant(uint value)
        {
            Value = value;
            Type = VariantType.UInt32;
            ArrayDimensions = null;
        }

        public Variant(long value)
        {
            Value = value;
            Type = VariantType.Int64;
            ArrayDimensions = null;
        }

        public Variant(ulong value)
        {
            Value = value;
            Type = VariantType.UInt64;
            ArrayDimensions = null;
        }

        public Variant(float value)
        {
            Value = value;
            Type = VariantType.Float;
            ArrayDimensions = null;
        }

        public Variant(double value)
        {
            Value = value;
            Type = VariantType.Double;
            ArrayDimensions = null;
        }

        public Variant(string? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.String;
            ArrayDimensions = null;
        }

        public Variant(DateTime value)
        {
            Value = value;
            Type = VariantType.DateTime;
            ArrayDimensions = null;
        }

        public Variant(Guid value)
        {
            Value = value;
            Type = VariantType.Guid;
            ArrayDimensions = null;
        }

        public Variant(byte[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ByteString;
            ArrayDimensions = null;
        }

        public Variant(XElement? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.XmlElement;
            ArrayDimensions = null;
        }

        public Variant(NodeId? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.NodeId;
            ArrayDimensions = null;
        }

        public Variant(ExpandedNodeId? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ExpandedNodeId;
            ArrayDimensions = null;
        }

        public Variant(StatusCode value)
        {
            Value = value;
            Type = VariantType.StatusCode;
            ArrayDimensions = null;
        }

        public Variant(QualifiedName? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.QualifiedName;
            ArrayDimensions = null;
        }

        public Variant(LocalizedText? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.LocalizedText;
            ArrayDimensions = null;
        }

        public Variant(ExtensionObject? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ExtensionObject;
            ArrayDimensions = null;
        }

        public Variant(IEncodable? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = new ExtensionObject(value);
            Type = VariantType.ExtensionObject;
            ArrayDimensions = null;
        }

        public Variant(Enum value)
        {
            Value = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            Type = VariantType.Int32;
            ArrayDimensions = null;
        }

        public Variant(bool[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Boolean;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(sbyte[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.SByte;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(short[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Int16;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(ushort[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.UInt16;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(int[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Int32;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(uint[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.UInt32;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(long[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Int64;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(ulong[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.UInt64;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(float[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Float;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(double[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Double;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(string?[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.String;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(DateTime[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.DateTime;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(Guid[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Guid;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(byte[]?[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ByteString;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(XElement?[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.XmlElement;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(NodeId[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.NodeId;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(ExpandedNodeId[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ExpandedNodeId;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(StatusCode[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.StatusCode;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(QualifiedName[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.QualifiedName;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(LocalizedText[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.LocalizedText;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(ExtensionObject?[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.ExtensionObject;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(Variant[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            Type = VariantType.Variant;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(Enum[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value.Select(v => Convert.ToInt32(v, CultureInfo.InvariantCulture)).ToArray();
            Type = VariantType.Int32;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(IEncodable[]? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value.Select(v => new ExtensionObject(v)).ToArray();
            Type = VariantType.ExtensionObject;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public Variant(Array? value)
        {
            if (value is null)
            {
                Value = null;
                Type = VariantType.Null;
                ArrayDimensions = null;
                return;
            }

            Value = value;
            VariantType varType;
            Type? elemType = value.GetType().GetElementType();
            if (elemType == null || !elemTypeMap.TryGetValue(elemType, out varType))
            {
                throw new ArgumentOutOfRangeException(nameof(value), elemType, "Array element type is unsupported.");
            }

            Type = varType;
            ArrayDimensions = new int[value.Rank];
            for (int i = 0; i < value.Rank; i++)
            {
                ArrayDimensions[i] = value.GetLength(i);
            }
        }

        public object? Value { get;}

        public VariantType Type { get; }

        public int[]? ArrayDimensions { get; }

        public static implicit operator Variant(bool value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(sbyte value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(byte value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(short value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ushort value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(int value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(uint value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(long value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ulong value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(float value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(double value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(string? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(DateTime value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(Guid value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(byte[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(XElement? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(NodeId? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ExpandedNodeId? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(StatusCode value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(QualifiedName? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(LocalizedText? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ExtensionObject? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(bool[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(sbyte[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(short[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ushort[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(int[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(uint[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(long[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ulong[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(float[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(double[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(string[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(DateTime[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(Guid[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(byte[][]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(XElement[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(NodeId[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ExpandedNodeId[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(StatusCode[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(QualifiedName[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(LocalizedText[]? value)
        {
            return new Variant(value);
        }

        public static implicit operator Variant(ExtensionObject[]? value)
        {
            return new Variant(value);
        }

        public static explicit operator bool(Variant value)
        {
            return (bool)value.Value!;
        }

        public static explicit operator sbyte(Variant value)
        {
            return (sbyte)value.Value!;
        }

        public static explicit operator byte(Variant value)
        {
            return (byte)value.Value!;
        }

        public static explicit operator short(Variant value)
        {
            return (short)value.Value!;
        }

        public static explicit operator ushort(Variant value)
        {
            return (ushort)value.Value!;
        }

        public static explicit operator int(Variant value)
        {
            return (int)value.Value!;
        }

        public static explicit operator uint(Variant value)
        {
            return (uint)value.Value!;
        }

        public static explicit operator long(Variant value)
        {
            return (long)value.Value!;
        }

        public static explicit operator ulong(Variant value)
        {
            return (ulong)value.Value!;
        }

        public static explicit operator float(Variant value)
        {
            return (float)value.Value!;
        }

        public static explicit operator double(Variant value)
        {
            return (double)value.Value!;
        }

        public static explicit operator string?(Variant value)
        {
            return (string?)value.Value;
        }

        public static explicit operator DateTime(Variant value)
        {
            return (DateTime)value.Value!;
        }

        public static explicit operator Guid(Variant value)
        {
            return (Guid)value.Value!;
        }

        public static explicit operator byte[]?(Variant value)
        {
            return (byte[]?)value.Value;
        }

        public static explicit operator XElement?(Variant value)
        {
            return (XElement?)value.Value;
        }

        public static explicit operator NodeId?(Variant value)
        {
            return (NodeId?)value.Value;
        }

        public static explicit operator ExpandedNodeId?(Variant value)
        {
            return (ExpandedNodeId?)value.Value;
        }

        public static explicit operator StatusCode(Variant value)
        {
            return (StatusCode)value.Value!;
        }

        public static explicit operator QualifiedName?(Variant value)
        {
            return (QualifiedName?)value.Value;
        }

        public static explicit operator LocalizedText?(Variant value)
        {
            return (LocalizedText?)value.Value;
        }

        public static explicit operator ExtensionObject?(Variant value)
        {
            return (ExtensionObject?)value.Value;
        }

        public static explicit operator bool[]?(Variant value)
        {
            return (bool[]?)value.Value;
        }

        public static explicit operator sbyte[]?(Variant value)
        {
            return (sbyte[]?)value.Value;
        }

        public static explicit operator short[]?(Variant value)
        {
            return (short[]?)value.Value;
        }

        public static explicit operator ushort[]?(Variant value)
        {
            return (ushort[]?)value.Value;
        }

        public static explicit operator int[]?(Variant value)
        {
            return (int[]?)value.Value;
        }

        public static explicit operator uint[]?(Variant value)
        {
            return (uint[]?)value.Value;
        }

        public static explicit operator long[]?(Variant value)
        {
            return (long[]?)value.Value;
        }

        public static explicit operator ulong[]?(Variant value)
        {
            return (ulong[]?)value.Value;
        }

        public static explicit operator float[]?(Variant value)
        {
            return (float[]?)value.Value;
        }

        public static explicit operator double[]?(Variant value)
        {
            return (double[]?)value.Value;
        }

        public static explicit operator string[]?(Variant value)
        {
            return (string[]?)value.Value;
        }

        public static explicit operator DateTime[]?(Variant value)
        {
            return (DateTime[]?)value.Value;
        }

        public static explicit operator Guid[]?(Variant value)
        {
            return (Guid[]?)value.Value;
        }

        public static explicit operator byte[][]?(Variant value)
        {
            return (byte[][]?)value.Value;
        }

        public static explicit operator XElement[]?(Variant value)
        {
            return (XElement[]?)value.Value;
        }

        public static explicit operator NodeId[]?(Variant value)
        {
            return (NodeId[]?)value.Value;
        }

        public static explicit operator ExpandedNodeId[]?(Variant value)
        {
            return (ExpandedNodeId[]?)value.Value;
        }

        public static explicit operator StatusCode[]?(Variant value)
        {
            return (StatusCode[]?)value.Value;
        }

        public static explicit operator QualifiedName[]?(Variant value)
        {
            return (QualifiedName[]?)value.Value;
        }

        public static explicit operator LocalizedText[]?(Variant value)
        {
            return (LocalizedText[]?)value.Value;
        }

        public static explicit operator ExtensionObject[]?(Variant value)
        {
            return (ExtensionObject[]?)value.Value;
        }

        public static bool IsNull(Variant a)
        {
            return a.Type == VariantType.Null || a.Value == null;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "{null}";
        }
    }
}