using Adita.PlexNet.ComponentModel.DataAnnotations.DataAnnotations;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Adita.PlexNet.Opc.Ua.Samples.ViewModels;

[Subscription("Main")]
public partial class SecondaryViewModel : SubscriptionBase
{
    [ObservableProperty]
    [MonitoredItem("ns=4;s=|var|CODESYS Control Win V3 x64.Application.gMain.lrValue")]
    [Validate]
    [CompareRange(nameof(MinValue), nameof(MaxValue))]
    [NotifyDataErrorInfo]
    private double _value;
    [ObservableProperty]
    private double _minValue;
    [ObservableProperty]
    private double _maxValue = 20;
    public SecondaryViewModel()
    {
    }
}
