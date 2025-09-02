using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

[assembly: TypeLibrary]
namespace Adita.PlexNet.Opc.Ua.Samples.Models;

[BinaryEncodingId("nsu=urn:Lenze:PLCOpen;s=|enc|c500.Application.TestModel")]
[DataTypeId("nsu=urn:Lenze:PLCOpen;s=|type|c500.Application.TestModel")]
public partial class TestModel : Structure
{
    [ObservableProperty]
    private double _value;

    public override bool IsDefault => Value == default;

    public override void Decode(IDecoder decoder)
    {
        decoder.PushNamespace("urn:Lenze:PLCOpen");
        Value = decoder.ReadDouble("lrValue");
        decoder.PopNamespace();
    }
    public override void Encode(IEncoder encoder)
    {
        encoder.PushNamespace("urn:Lenze:PLCOpen");
        encoder.WriteDouble("lrValue", Value);
        encoder.PopNamespace();
    }
}
