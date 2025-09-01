using Adita.PlexNet.Opc.Ua.Annotations;
using Adita.PlexNet.Opc.Ua.Samples.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Adita.PlexNet.Opc.Ua.Samples.ViewModels;

[Subscription("Main")]
public partial class MainViewModel : SubscriptionBase
{
    [ObservableProperty]
    [MonitoredItem("ns=4;s=|var|c500.Application.gMain.stTestModel")]
    private TestModel _testModel;
    public MainViewModel()
    {

    }

    [RelayCommand]
    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}

