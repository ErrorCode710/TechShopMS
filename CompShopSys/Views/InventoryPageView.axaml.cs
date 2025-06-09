using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompShopSys.ViewModels;

namespace CompShopSys.Views;

public partial class InventoryPageView : UserControl
{
    public InventoryPageView()
    {
        InitializeComponent();
        DataContext = new InventoryPageViewModel();
    }
}