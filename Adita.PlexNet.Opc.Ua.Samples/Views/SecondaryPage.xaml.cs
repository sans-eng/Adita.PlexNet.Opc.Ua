using Adita.PlexNet.Opc.Ua.Samples.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Adita.PlexNet.Opc.Ua.Samples.Views;

public sealed partial class SecondaryPage : Page
{
    public SecondaryViewModel ViewModel
    {
        get;
    }

    public SecondaryPage()
    {
        ViewModel = App.GetService<SecondaryViewModel>();
        InitializeComponent();
    }
}
