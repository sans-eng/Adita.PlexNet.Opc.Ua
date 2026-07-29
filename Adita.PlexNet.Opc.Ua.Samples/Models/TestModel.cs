using Adita.PlexNet.Opc.Ua.Abstractions.Decoders;
using Adita.PlexNet.Opc.Ua.Abstractions.Encoders;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

[assembly: TypeLibrary]
namespace Adita.PlexNet.Opc.Ua.Samples.Models;

[BinaryEncodingId("nsu=urn:Lenze:PLCOpen;s=|enc|c500.Application.TestModel")]
[DataTypeId("nsu=urn:Lenze:PLCOpen;s=|type|c500.Application.TestModel")]
public partial class TestModel : ObservableObject
{
    [ObservableProperty]
    private double _value;

    public  void Decode(IDecoder decoder)
    {
        decoder.PushNamespace("urn:Lenze:PLCOpen");
        decoder.PopNamespace();
    }
    public  void Encode(IEncoder encoder)
    {
        encoder.PushNamespace("urn:Lenze:PLCOpen");
        encoder.PopNamespace();
    }
}
