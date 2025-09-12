using System.Reflection;
using System.Xml.Linq;
using Adita.PlexNet.Opc.Ua.Abstractions.Encodables;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;

namespace Adita.PlexNet.Opc.Ua.Extensions;
public static class EncoderExtensions
{
    #region Public methods
    public static void Write(this IEncoder encoder, object? value, string fieldName)
    {
        ArgumentNullException.ThrowIfNull(encoder);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);

        if (value is bool booleanValue)
        {
            encoder.WriteBoolean(fieldName, booleanValue);
        }
        else if (value is sbyte sbyteValue)
        {
            encoder.WriteSByte(fieldName, sbyteValue);
        }
        else if (value is byte byteValue)
        {
            encoder.WriteByte(fieldName, byteValue);
        }
        else if (value is short shortValue)
        {
            encoder.WriteInt16(fieldName, shortValue);
        }
        else if (value is ushort ushortValue)
        {
            encoder.WriteUInt16(fieldName, ushortValue);
        }
        else if (value is int intValue)
        {
            encoder.WriteInt32(fieldName, intValue);
        }
        else if (value is uint uintValue)
        {
            encoder.WriteUInt32(fieldName, uintValue);
        }
        else if (value is long longValue)
        {
            encoder.WriteInt64(fieldName, longValue);
        }
        else if (value is ulong ulongValue)
        {
            encoder.WriteUInt64(fieldName, ulongValue);
        }
        else if (value is float floatValue)
        {
            encoder.WriteFloat(fieldName, floatValue);
        }
        else if (value is double doubleValue)
        {
            encoder.WriteDouble(fieldName, doubleValue);
        }
        else if (value is string stringValue)
        {
            encoder.WriteString(fieldName, stringValue);
        }
        else if (value is DateTime dateTimeValue)
        {
            encoder.WriteDateTime(fieldName, dateTimeValue);
        }
        else if (value is Guid guidValue)
        {
            encoder.WriteGuid(fieldName, guidValue);
        }
        else if (value is XElement xElement)
        {
            encoder.WriteXElement(fieldName, xElement);
        }
        else if (value is NodeId nodeId)
        {
            encoder.WriteNodeId(fieldName, nodeId);
        }
        else if (value is ExpandedNodeId expandedNodeId)
        {
            encoder.WriteExpandedNodeId(fieldName, expandedNodeId);
        }
        else if (value is StatusCode statusCodeValue)
        {
            encoder.WriteStatusCode(fieldName, statusCodeValue);
        }
        else if (value is QualifiedName qualifiedNameValue)
        {
            encoder.WriteQualifiedName(fieldName, qualifiedNameValue);
        }
        else if (value is LocalizedText localizedTextValue)
        {
            encoder.WriteLocalizedText(fieldName, localizedTextValue);
        }
        else if (value is ExtensionObject extensionObjectValue)
        {
            encoder.WriteExtensionObject(fieldName, extensionObjectValue);
        }
        else if (value is DataValue dataValue)
        {
            encoder.WriteDataValue(fieldName, dataValue);
        }
        else if (value is Variant variantValue)
        {
            encoder.WriteVariant(fieldName, variantValue);
        }
        else if (value is DiagnosticInfo diagnosticInfoValue)
        {
            encoder.WriteDiagnosticInfo(fieldName, diagnosticInfoValue);
        }
        else if (value is IEncodable encodableValue)
        {
            var methodInfo = encoder.GetType().GetMethod(nameof(encoder.WriteEncodable));
            WriteGenericMethod(methodInfo, encoder, fieldName, encodableValue);
        }
        else if (value?.GetType().IsEnum == true)
        {
            var methodInfo = encoder.GetType().GetMethod(nameof(encoder.WriteEnumeration));
            WriteGenericMethod(methodInfo, encoder, fieldName, value);
        }
        else if (value is bool[] booleanArrayValue)
        {
            encoder.WriteBooleanArray(fieldName, booleanArrayValue);
        }
        else if (value is sbyte[] sbyteArrayValue)
        {
            encoder.WriteSByteArray(fieldName, sbyteArrayValue);
        }
        else if (value is byte[] byteArrayValue)
        {
            encoder.WriteByteArray(fieldName, byteArrayValue);
        }
        else if (value is short[] shortArrayValue)
        {
            encoder.WriteInt16Array(fieldName, shortArrayValue);
        }
        else if (value is ushort[] ushortArrayValue)
        {
            encoder.WriteUInt16Array(fieldName, ushortArrayValue);
        }
        else if (value is int[] intArrayValue)
        {
            encoder.WriteInt32Array(fieldName, intArrayValue);
        }
        else if (value is uint[] uintArrayValue)
        {
            encoder.WriteUInt32Array(fieldName, uintArrayValue);
        }
        else if (value is long[] longArrayValue)
        {
            encoder.WriteInt64Array(fieldName, longArrayValue);
        }
        else if (value is ulong[] ulongArrayValue)
        {
            encoder.WriteUInt64Array(fieldName, ulongArrayValue);
        }
        else if (value is float[] floatArrayValue)
        {
            encoder.WriteFloatArray(fieldName, floatArrayValue);
        }
        else if (value is double[] doubleArrayValue)
        {
            encoder.WriteDoubleArray(fieldName, doubleArrayValue);
        }
        else if (value is string[] stringArrayValue)
        {
            encoder.WriteStringArray(fieldName, stringArrayValue);
        }
        else if (value is DateTime[] dateTimeArrayValue)
        {
            encoder.WriteDateTimeArray(fieldName, dateTimeArrayValue);
        }
        else if (value is Guid[] guidArrayValue)
        {
            encoder.WriteGuidArray(fieldName, guidArrayValue);
        }
        else if (value is byte[][] byteStringArray)
        {
            encoder.WriteByteStringArray(fieldName, byteStringArray);
        }
        else if (value is XElement[] xElementArrayValue)
        {
            encoder.WriteXElementArray(fieldName, xElementArrayValue);
        }
        else if (value is NodeId[] nodeIdArrayValue)
        {
            encoder.WriteNodeIdArray(fieldName, nodeIdArrayValue);
        }
        else if (value is ExpandedNodeId[] expandedNodeIdArrayValue)
        {
            encoder.WriteExpandedNodeIdArray(fieldName, expandedNodeIdArrayValue);
        }
        else if (value is StatusCode[] statusCodeArrayValue)
        {
            encoder.WriteStatusCodeArray(fieldName, statusCodeArrayValue);
        }
        else if (value is QualifiedName[] qualifiedQualifiedNameArrayValue)
        {
            encoder.WriteQualifiedNameArray(fieldName, qualifiedQualifiedNameArrayValue);
        }
        else if (value is LocalizedText[] localizedTextArrayValue)
        {
            encoder.WriteLocalizedTextArray(fieldName, localizedTextArrayValue);
        }
        else if (value is ExtensionObject[] extensionsArrayValue)
        {
            encoder.WriteExtensionObjectArray(fieldName, extensionsArrayValue);
        }
        else if (value is DataValue[] dataValueArrayValue)
        {
            encoder.WriteDataValueArray(fieldName, dataValueArrayValue);
        }
        else if (value is Variant[] variantArrayValue)
        {
            encoder.WriteVariantArray(fieldName, variantArrayValue);
        }
        else if (value is DiagnosticInfo[] diagnosticInfoArrayValue)
        {
            encoder.WriteDiagnosticInfoArray(fieldName, diagnosticInfoArrayValue);
        }
        else if (value is IEncodable[] iEncodableArrayValue)
        {
            var methodInfo = encoder.GetType().GetMethod(nameof(encoder.WriteEncodableArray));
            WriteGenericMethod(methodInfo, encoder, fieldName, iEncodableArrayValue);
        }
        else if (value?.GetType().IsArray == true && value?.GetType().GetElementType()?.IsEnum == true)
        {
            var methodInfo = encoder.GetType().GetMethod(nameof(encoder.WriteEnumerationArray));
            WriteGenericMethod(methodInfo, encoder, fieldName, value);
        }

    }
    #endregion Public methods

    #region Private methods
    private static void WriteGenericMethod(MethodInfo? methodInfo, IEncoder encoder, string fieldName, object value)
    {
        if (methodInfo?.IsGenericMethod == true)
        {
            var genericMethodInfo = methodInfo.MakeGenericMethod(value.GetType());
            genericMethodInfo.Invoke(encoder, [fieldName, value]);
        }
    }
    #endregion Private methods
}
