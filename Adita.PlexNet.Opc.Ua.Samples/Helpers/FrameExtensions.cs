using Microsoft.UI.Xaml.Controls;

namespace Adita.PlexNet.Opc.Ua.Samples.Helpers;

public static class FrameExtensions
{
    public static object? GetPageViewModel(this Microsoft.UI.Xaml.Controls.Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}
