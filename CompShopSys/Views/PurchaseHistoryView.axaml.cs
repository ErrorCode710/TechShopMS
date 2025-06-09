using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace TechShopMS.Views;

public partial class PurchaseHistoryView : AppWindow
{
    public PurchaseHistoryView()
    {
        InitializeComponent();
        //DataContext = new PurchaseHistoryViewModel();
    }
}   