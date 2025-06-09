using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class InventoryPageView : UserControl
{
    public InventoryPageView()
    {
        InitializeComponent();
        DataContext = new InventoryPageViewModel();
    }
}