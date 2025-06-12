using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
    private void AutoCompleteBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is AutoCompleteBox box)
        {
            // This makes sure the dropdown opens even if no text is typed
            box.IsDropDownOpen = true;
        }
    }
}