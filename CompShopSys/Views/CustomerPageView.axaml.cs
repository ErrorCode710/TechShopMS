using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class CustomerPageView : UserControl
{
    public CustomerPageView()
    {
        InitializeComponent();
        DataContext = new CustomerPageViewModel();
    }
}