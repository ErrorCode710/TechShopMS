using Avalonia.Controls;
using Avalonia.Styling;
using CompShopSys.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace CompShopSys.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        DataContext = new MainWindowViewModel();
        this.RequestedThemeVariant = ThemeVariant.Light;
    }

}