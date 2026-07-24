using System.Collections.ObjectModel;
using Adita.PlexNet.Opc.Ua.Annotations;
using Adita.PlexNet.Opc.Ua.Extensions;
using Adita.PlexNet.Opc.Ua.Samples.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Adita.PlexNet.Opc.Ua.Samples.ViewModels;

[Subscription("Main", isBatched:true)]
public partial class MainViewModel : SubscriptionBase
{
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"lrValue\"")]
    private double _doublevalue;
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"lrValue1\"")]
    private double _doublevalue1;
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"lrValue2\"")]
    private double _doublevalue2;
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"lrValue3\"")]
    private double _doublevalue3;
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"lrValue4\"")]
    private double _doublevalu4;
    [ObservableProperty]
    [MonitoredItem("ns=3;s=\"dbMain\".\"iValue\"")]
    private int _intValue;
    [ObservableProperty]
    private int _monitoredItemsCount;

    [RelayCommand]
    private async Task GetMonitoredItemsAsync()
    {
        var inputArguments = new Variant[] {SubscriptionId};
        var callMethodRequest = new CallMethodRequest
        {
            MethodId = NodeId.Parse(MethodIds.ServerType_GetMonitoredItems),
            ObjectId = NodeId.Parse(ObjectIds.Server),
            InputArguments = inputArguments
        };

        var request = new CallRequest
        {
            MethodsToCall = [callMethodRequest]
        };

        var response = await InnerChannel.CallAsync(request);
        if (response?.ResponseHeader?.ServiceResult is StatusCode statusCode && StatusCode.IsGood(statusCode) && response?.Results?.Length > 0)
        {
            var outputArguments = response.Results[0]?.OutputArguments;
            if (outputArguments?.Length > 0)
            {
                var serverHandle = outputArguments[0];
                var monitoredItemIds = serverHandle.Value as uint[];
                MonitoredItemsCount = monitoredItemIds?.Length ?? 0;
            }
        }
    }
    [RelayCommand]
    private async Task GetSubscriptionsAsync()
    {
        var inputArguments = new Variant[] { SubscriptionId };
        var callMethodRequest = new CallMethodRequest
        {
            MethodId = NodeId.Parse(MethodIds.ServerType_SetSubscriptionDurable),
            ObjectId = NodeId.Parse(ObjectIds.Server),
            InputArguments = inputArguments
        };

        var request = new CallRequest
        {
            MethodsToCall = [callMethodRequest]
        };

        var response = await InnerChannel.CallAsync(request);
        if (response?.ResponseHeader?.ServiceResult is StatusCode statusCode && StatusCode.IsGood(statusCode) && response?.Results?.Length > 0)
        {
            var outputArguments = response.Results[0]?.OutputArguments;
            if (outputArguments?.Length > 0)
            {
                var serverHandle = outputArguments[0];
                var monitoredItemIds = serverHandle.Value as uint[];
                MonitoredItemsCount = monitoredItemIds?.Length ?? 0;
            }
        }
    }

    [RelayCommand]
    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}

