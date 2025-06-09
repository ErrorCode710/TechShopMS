using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompShopSys.ViewModels;

namespace CompShopSys.Views;

public partial class SalePageView : UserControl
{
    public SalePageView()
    {
        InitializeComponent();
        var vm = new SalePageViewModel();
        DataContext = vm;
        
    }
}