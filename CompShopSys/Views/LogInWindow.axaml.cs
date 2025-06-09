
using Avalonia.Styling;
using TechShopMS.Services;
using FluentAvalonia.UI.Windowing;
using TechShopMS.ViewModels;
using Avalonia.Threading;
using System.Diagnostics;


namespace TechShopMS.Views;

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