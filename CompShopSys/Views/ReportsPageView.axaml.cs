using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompShopSys.ViewModels;

namespace CompShopSys.Views;

public partial class ReportsPageView : UserControl
{
    public ReportsPageView()
    {
        InitializeComponent();
        DataContext = new ReportsPageViewModel();
    }
}