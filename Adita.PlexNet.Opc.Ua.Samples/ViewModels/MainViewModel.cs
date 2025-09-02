using System.Collections.ObjectModel;
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



    private List<TestModel> _testModels;
    private List<double> _testValues;

    [MonitoredItem("ns=4;s=|var|c500.Application.gMain.astTestModel")]
    public List<TestModel> TestModels
    {
        get => _testModels;
        set => SetProperty(ref _testModels, value);
    }
    [MonitoredItem("ns=4;s=|var|c500.Application.gMain.alrTest")]
    public List<double> TestValues
    {
        get => _testValues;
        set => SetProperty(ref _testValues, value);
    }
    public MainViewModel()
    {

    }

    [RelayCommand]
    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}

