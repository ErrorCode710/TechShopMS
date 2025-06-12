using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using TechShopMS.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
namespace TechShopMS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase

    {

        [ObservableProperty] private string fullName;
        [ObservableProperty] private string email;
        [ObservableProperty] private string userRoleDisplay;
        [ObservableProperty] private string _headerLabel = "Home";


        [ObservableProperty] private ViewModelBase _currentPage = new HomePageViewModel();


        [ObservableProperty] private ListItemTemplate? _selectedListItem;

        public ObservableCollection<ListItemTemplate> Items { get; } = new();

        async partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            Debug.WriteLine($"isEnabled {value.IsEnabled}");
            if (!value.IsEnabled)
            {
                var confirmBox = MessageBoxManager.GetMessageBoxStandard(
               "Access Denied",
               $"Only for Admin Role ",
               ButtonEnum.Ok,
               Icon.Warning);

                var result = await confirmBox.ShowAsync();
            }

            if (value is null || !value.IsEnabled)
                return;

            var instance = Activator.CreateInstance(value.ModelType);
            if (instance is null) return;

            CurrentPage = (ViewModelBase)instance;
            HeaderLabel = value.Label;
        }

        public MainWindowViewModel()
        { }

        public void SetUpNavigationItems() {
            //Items.Add(new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"));
            Items.Add(new ListItemTemplate(typeof(ProductCatalogPageViewModel), "InventoryRegular"));
            Items.Add(new ListItemTemplate(typeof(SalePageViewModel), "SalesRegular"));
            Items.Add(new ListItemTemplate(typeof(CustomerPageViewModel), "CustomerRegular"));
            Items.Add(new ListItemTemplate(typeof(ReportsPageViewModel), "ReportRegular"));

            Debug.WriteLine($"User Role Display: {UserRoleDisplay}");
            Debug.WriteLine($"Email Display: {Email}");
           

            bool canAccessUserManagement = UserRoleDisplay == "Admin";

            Items.Add(new ListItemTemplate(typeof(UserManagementPageViewModel), "UserRegular", canAccessUserManagement));
        }

        public void userDetailsDisplay()
        {

        }
      //  public ObservableCollection<ListItemTemplate> Items { get; } = new()
      //{
      //    new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular"),
      //    new ListItemTemplate(typeof(ProductCatalogPageViewModel), "InventoryRegular"),
      //    new ListItemTemplate(typeof(SalesPageViewModel), "SalesRegular"),
      //    new ListItemTemplate(typeof(CustomerPageViewModel), "CustomerRegular"),
      //    new ListItemTemplate(typeof(ReportsPageViewModel), "ReportRegular"),
      //    new ListItemTemplate(typeof(UserManagementPageViewModel), "UserRegular", UserRoleDisplay == "Admin")



      //};

      
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string iconKey, bool isEnabled = true)
        {
            ModelType = type;
            IsEnabled = isEnabled;

            var rawLabel = type.Name.Replace("PageViewModel", "");
            Label = Regex.Replace(rawLabel, "(?<!^)([A-Z])", " $1").Trim();

            Application.Current!.TryFindResource(iconKey, out var res);
            ListItemIcon = (StreamGeometry)res!;


        }
        public string Label { get; set; }
        public Type ModelType { get;  }
        public StreamGeometry ListItemIcon { get; }
        public bool IsEnabled { get; }



    }
}
