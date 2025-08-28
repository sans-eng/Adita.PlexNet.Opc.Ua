using System.ComponentModel.DataAnnotations;
using Adita.PlexNet.ComponentModel.DataAnnotations.DataAnnotations;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua.Sample.ViewModels;

[Subscription("Main")]
public partial class MainViewModel : SubscriptionBase
{
    [ObservableProperty]
    [MonitoredItem("ns=4;s=|var|CODESYS Control Win V3 x64.Application.gMain.lrValue")]
    [Validate]
    [CompareRange(nameof(MinValue), nameof(MaxValue))]
    private double _value;
    [ObservableProperty]
    private double _minValue;
    [ObservableProperty]
    private double _maxValue = 20;
    public MainViewModel()
    {
    }
}
