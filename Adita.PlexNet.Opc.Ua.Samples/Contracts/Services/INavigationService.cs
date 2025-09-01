using Microsoft.UI.Xaml.Navigation;

namespace Adita.PlexNet.Opc.Ua.Samples.Contracts.Services;

public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack
    {
        get;
    }

    Microsoft.UI.Xaml.Controls.Frame? Frame
    {
        get; set;
    }

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    bool GoBack();
}
