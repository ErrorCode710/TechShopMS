using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class ProductCatalogPageView : UserControl
{
    public ProductCatalogPageView()
    {
        InitializeComponent();
        DataContext = new ProductCatalogPageViewModel();
    }
}