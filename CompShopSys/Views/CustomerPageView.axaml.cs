using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompShopSys.ViewModels;

namespace CompShopSys.Views;

public partial class CustomerPageView : UserControl
{
    public CustomerPageView()
    {
        InitializeComponent();
        DataContext = new CustomerPageViewModel();
    }
}