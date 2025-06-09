using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompShopSys.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace CompShopSys.Views;

public partial class PurchaseHistoryView : AppWindow
{
    public PurchaseHistoryView()
    {
        InitializeComponent();
        //DataContext = new PurchaseHistoryViewModel();
    }
}   