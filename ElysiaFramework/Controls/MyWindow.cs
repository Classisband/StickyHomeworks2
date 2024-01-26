using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using ElysiaFramework.Interfaces;
using static ElysiaFramework.NativeWindowHelper;

namespace ElysiaFramework.Controls;


public class MyWindow : Window
{
    private IThemeService? ThemeService { get; }

    public MyWindow()
    {
        try
        {
            ThemeService = AppEx.GetService<IThemeService>();
            ThemeService.ThemeUpdated += ThemeServiceOnThemeUpdated;
        }
        catch
        {
            // ignored
        }
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateImmersiveDarkMode(ThemeService?.CurrentRealThemeMode ?? 0);
    }

    private void ThemeServiceOnThemeUpdated(object? sender, ThemeUpdatedEventArgs e)
    {
        UpdateImmersiveDarkMode(e.RealThemeMode);
    }

    private void UpdateImmersiveDarkMode(int mode)
    {
        var trueVal = 0x01;
        var falseVal = 0x00;
        if (mode == 0)
        {
            DwmSetWindowAttribute(new WindowInteropHelper(this).Handle,
                DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref falseVal,
                Marshal.SizeOf(typeof(int)));
        }
        else
        {
            DwmSetWindowAttribute(new WindowInteropHelper(this).Handle,
                DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref trueVal,
                Marshal.SizeOf(typeof(int)));
        }
    }
}