using Adita.PlexNet.Opc.Ua.Sample.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Adita.PlexNet.Opc.Ua.Sample.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
