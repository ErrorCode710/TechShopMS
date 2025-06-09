
using Avalonia.Styling;
using CompShopSys.Services;
using FluentAvalonia.UI.Windowing;
using CompShopSys.ViewModels;
using Avalonia.Threading;
using System.Diagnostics;


namespace CompShopSys.Views;

public partial class LogInWindow : AppWindow
{
    private LogInWindowViewModel _viewModel;
    public LogInWindow()
    {
        InitializeComponent();
        this.RequestedThemeVariant = ThemeVariant.Light;


        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;

        _viewModel = new LogInWindowViewModel(this);
        _viewModel.init();

        
        DataContext = _viewModel;
       
        


    }
    public void CloseLoginWindow()
    {
        this.Close(); 
    }
}