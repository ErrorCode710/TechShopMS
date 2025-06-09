using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class ReportsPageView : UserControl
{
    public ReportsPageView()
    {
        InitializeComponent();
        DataContext = new ReportsPageViewModel();
    }
}