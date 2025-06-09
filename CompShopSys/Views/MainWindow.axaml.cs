using Avalonia.Controls;
using Avalonia.Styling;
using TechShopMS.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace TechShopMS.Views;

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