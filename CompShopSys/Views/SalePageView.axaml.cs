using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class SalePageView : UserControl
{
    public SalePageView()
    {
        InitializeComponent();
        var vm = new SalePageViewModel();
        DataContext = vm;
        
    }
}