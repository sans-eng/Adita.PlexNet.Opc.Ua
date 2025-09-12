using System.Xml.Linq;
using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;

namespace Adita.PlexNet.Opc.Ua.Extensions;
public static class DecoderExtensions
{
    #region Public methods
    public static object? Read(this IDecoder decoder, Type type, string fieldName)
    {
        ArgumentNullException.ThrowIfNull(decoder);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);

        if (type == typeof(bool))
        {
            return decoder.ReadBoolean(fieldName);
        }
        else if (type == typeof(sbyte))
        {
            return decoder.ReadSByte(fieldName);
        }
        else if (type == typeof(byte))
        {
            return decoder.ReadByte(fieldName);
        }
        else if (type == typeof(short))
        {
            return decoder.ReadInt16(fieldName);
        }
        else if (type == typeof(ushort))
        {
            return decoder.ReadUInt16(fieldName);
        }
        else if (type == typeof(int))
        {
            return decoder.ReadInt32(fieldName);
        }
        else if (type == typeof(uint))
        {
            return decoder.ReadUInt32(fieldName);
        }
        else if (type == typeof(long))
        {
            return decoder.ReadInt64(fieldName);
        }
        else if (type == typeof(ulong))
        {
            return decoder.ReadUInt64(fieldName);
        }
        else if (type == typeof(float))
        {
            return decoder.ReadFloat(fieldName);
        }
        else if (type == typeof(double))
        {
            return decoder.ReadDouble(fieldName);
        }
        else if (type == typeof(string))
        {
            return decoder.ReadString(fieldName);
        }
        else if (type == typeof(DateTime))
        {
            return decoder.ReadDateTime(fieldName);
        }
        else if (type == typeof(Guid))
        {
            return decoder.ReadGuid(fieldName);
        }
        else if (type == typeof(byte[]))
        {
            return decoder.ReadByteArray(fieldName);
        }
        else if (type == typeof(XElement))
        {
            return decoder.ReadXElement(fieldName);
        }
        else if (type == typeof(NodeId))
        {
            return decoder.ReadNodeId(fieldName);
        }
        else if (type == typeof(ExpandedNodeId))
        {
            return decoder.ReadExpandedNodeId(fieldName);
        }
        else if (type == typeof(StatusCode))
        {
            return decoder.ReadStatusCode(fieldName);
        }
        else if (type == typeof(QualifiedName))
        {
            return decoder.ReadQualifiedName(fieldName);
        }
        else if (type == typeof(LocalizedText))
        {
            return decoder.ReadLocalizedText(fieldName);
        }
        else if (type == typeof(ExtensionObject))
        {
            return decoder.ReadExtensionObject(fieldName);
        }
        else if (type == typeof(DataValue))
        {
            return decoder.ReadDataValue(fieldName);
        }
        else if (type == typeof(Variant))
        {
            return decoder.ReadVariant(fieldName);
        }
        else if (type == typeof(DiagnosticInfo))
        {
            return decoder.ReadDiagnosticInfo(fieldName);
        }
        else if (type.IsAssignableTo(typeof(IEncodable)))
        {
            var methodInfo = decoder.GetType().GetMethod(nameof(decoder.ReadEncodable))?.MakeGenericMethod(type);
            if (methodInfo != null)
            {
                return methodInfo.Invoke(decoder, [fieldName]);
            }
            else
            {
                return default;
            }
        }
        else if (type.IsEnum)
        {
            var methodInfo = decoder.GetType().GetMethod(nameof(decoder.ReadEnumeration))?.MakeGenericMethod(type);
            if (methodInfo != null)
            {
                return methodInfo.Invoke(decoder, [fieldName]);
            }
            else
            {
                return default;
            }
        }
        else if (type == typeof(bool[]))
        {
            return decoder.ReadBooleanArray(fieldName);
        }
        else if (type == typeof(sbyte[]))
        {
            return decoder.ReadSByteArray(fieldName);
        }
        else if (type == typeof(byte[]))
        {
            return decoder.ReadByteArray(fieldName);
        }
        else if (type == typeof(short[]))
        {
            return decoder.ReadInt16Array(fieldName);
        }
        else if (type == typeof(ushort[]))
        {
            return decoder.ReadUInt16Array(fieldName);
        }
        else if (type == typeof(int[]))
        {
            return decoder.ReadInt32Array(fieldName);
        }
        else if (type == typeof(uint[]))
        {
            return decoder.ReadUInt32Array(fieldName);
        }
        else if (type == typeof(long[]))
        {
            return decoder.ReadInt64Array(fieldName);
        }
        else if (type == typeof(ulong[]))
        {
            return decoder.ReadUInt64Array(fieldName);
        }
        else if (type == typeof(float[]))
        {
            return decoder.ReadFloatArray(fieldName);
        }
        else if (type == typeof(double[]))
        {
            return decoder.ReadDoubleArray(fieldName);
        }
        else if (type == typeof(string[]))
        {
            return decoder.ReadStringArray(fieldName);
        }
        else if (type == typeof(DateTime[]))
        {
            return decoder.ReadDateTimeArray(fieldName);
        }
        else if (type == typeof(Guid[]))
        {
            return decoder.ReadGuidArray(fieldName);
        }
        else if (type == typeof(byte[][]))
        {
            return decoder.ReadByteStringArray(fieldName);
        }
        else if (type == typeof(XElement[]))
        {
            return decoder.ReadXElementArray(fieldName);
        }
        else if (type == typeof(NodeId[]))
        {
            return decoder.ReadNodeIdArray(fieldName);
        }
        else if (type == typeof(ExpandedNodeId[]))
        {
            return decoder.ReadExpandedNodeIdArray(fieldName);
        }
        else if (type == typeof(StatusCode[]))
        {
            return decoder.ReadStatusCodeArray(fieldName);
        }
        else if (type == typeof(QualifiedName[]))
        {
            return decoder.ReadQualifiedNameArray(fieldName);
        }
        else if (type == typeof(LocalizedText[]))
        {
            return decoder.ReadLocalizedTextArray(fieldName);
        }
        else if (type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>) && i.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(IEncodable)) == true) is Type interfaceType)
        {
            var genericArgument = interfaceType.GetGenericArguments().FirstOrDefault();
            if (genericArgument != null)
            {
                var typeArray = genericArgument.MakeArrayType();
                var methodInfo = decoder.GetType().GetMethod(nameof(decoder.ReadEncodableArray))?.MakeGenericMethod(typeArray);
                if (methodInfo != null)
                {
                    return methodInfo.Invoke(decoder, [fieldName]);
                }
            }

            return default;
        }

        return default;
    }
    #endregion Public methods
}
