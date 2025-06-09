using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using TechShopMS.ViewModels;

namespace TechShopMS.Views;

public partial class UserManagementPageView : UserControl
{
    public UserManagementPageView()
    {
        InitializeComponent();
        DataContext = new UserManagementPageViewModel();
        
    }
}