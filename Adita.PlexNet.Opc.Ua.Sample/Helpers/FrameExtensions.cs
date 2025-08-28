using Microsoft.UI.Xaml.Controls;

namespace Adita.PlexNet.Opc.Ua.Sample.Helpers;
using Frame = Microsoft.UI.Xaml.Controls.Frame;

public static class FrameExtensions
{
    public static object? GetPageViewModel(this Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}
