using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using CompShopSys.ViewModels;

namespace CompShopSys.Views;

public partial class UserManagementPageView : UserControl
{
    public UserManagementPageView()
    {
        InitializeComponent();
        DataContext = new UserManagementPageViewModel();
        
    }
}