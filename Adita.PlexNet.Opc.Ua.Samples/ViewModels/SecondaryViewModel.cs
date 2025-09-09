using Adita.PlexNet.ComponentModel.DataAnnotations.DataAnnotations;
using Adita.PlexNet.Opc.Ua.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Adita.PlexNet.Opc.Ua.Samples.ViewModels;

[Subscription("Main")]
public partial class SecondaryViewModel : SubscriptionBase
{
    [ObservableProperty]
    [MonitoredItem("ns=4;s=|var|c500.Application.gMain.lrValue")]
    [Validate]
    [CompareRange(nameof(MinValue), nameof(MaxValue))]
    [NotifyDataErrorInfo]
    private double _value;
    [ObservableProperty]
    private double _minValue;
    [ObservableProperty]
    private double _maxValue = 20;

    [ObservableProperty]
    [MonitoredItem("ns=4;s=|var|c500.Application.gMain.lrValue")]
    [Validate]
    [CompareRange(nameof(MinValue), nameof(MaxValue))]
    [NotifyDataErrorInfo]
    private double _value1;
    public SecondaryViewModel()
    {
    }

    [RelayCommand]
    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
