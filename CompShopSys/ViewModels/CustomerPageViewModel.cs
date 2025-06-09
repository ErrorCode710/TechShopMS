using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CompShopSys.Models;
using CompShopSys.Services;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Diagnostics;
using CompShopSys.Views;
using Avalonia.Controls;
using Avalonia.VisualTree;


namespace CompShopSys.ViewModels
{
   public partial class CustomerPageViewModel  :ViewModelBase
    {
        #region Fields
        private readonly CustomerDbManager _dbManager;
        private const int PageSize = 5;

        public List<bool> BooleanValues { get; set; } = new List<bool>() { true, false };
        public bool SelectedBoolean { get; set; }
        #endregion

        #region Observable Properties
        [ObservableProperty]private ObservableCollection<Customer> customers = new();
        [ObservableProperty]private ObservableCollection<Customer> filteredCustomers = new();
        [ObservableProperty]private ObservableCollection<Customer> pagedCustomers = new();

        [ObservableProperty]private Customer selectedCustomer;
        [ObservableProperty]private string searchQuery;

        [ObservableProperty]private int currentPage = 1;
        [ObservableProperty]private int totalPages;


        [ObservableProperty] private int id;
        [ObservableProperty] private string firstName;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string contactNumber;
        [ObservableProperty] private string email;
        [ObservableProperty] private string address;
        [ObservableProperty] private DateTime dateCreated;
        [ObservableProperty] private bool isActive;


        #endregion

        #region Constructor
        public CustomerPageViewModel()
        {
            _dbManager = new CustomerDbManager("Data Source=data/CustomerDB.db");
            loadCustomer();
        }
        #endregion
        #region Search

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredCustomers = new ObservableCollection<Customer>(Customers);
            }
            else
            {
                string lowerQuery = SearchQuery.Trim().ToLower();

                var filtered = Customers.Where(u =>
                    (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.ToLower().Contains(lowerQuery)) ||
                   
                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(lowerQuery)) ||
                    u.IsActive.ToString().ToLower().Contains(lowerQuery)
                );

                FilteredCustomers = new ObservableCollection<Customer>(filtered);
            }

            CurrentPage = 1;
            ApplyPagination();
        }
        partial void OnSearchQueryChanged(string value)
        {  
            Search();
        }


        #endregion
        #region Pagination

        private void ApplyPagination()
        {
            if (filteredCustomers == null || filteredCustomers.Count == 0)
            {
                PagedCustomers = new ObservableCollection<Customer>();
                TotalPages = 1;
                return;
            }

            TotalPages = (int)Math.Ceiling((double)filteredCustomers.Count / PageSize);

            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);

            var paged = filteredCustomers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedCustomers = new ObservableCollection<Customer>(paged);
        }
        private void LoadPage(int pageNumber)
        {
            var pageItems = Customers
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedCustomers = new ObservableCollection<Customer>(pageItems);
        }
        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                //ApplyPagination();
                LoadPage(CurrentPage);
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPage(CurrentPage);
                //ApplyPagination();
            }
        }

        #endregion


        #region Load , Reset & Select 
        private void loadCustomer()
        {
            var allCustomers = _dbManager.GetAllCustomers();
            Customers = new ObservableCollection<Customer>(allCustomers);

            FilteredCustomers = new ObservableCollection<Customer>(Customers);
            TotalPages = (int)Math.Ceiling((double)Customers.Count / PageSize);
            CurrentPage = 1;
            //ApplyPagination();
            LoadPage(CurrentPage);

        }
        partial void OnSelectedCustomerChanged(Customer value)
        {
            if (value is not null)
            {
                Id = value.Id;
                FirstName = value.FirstName;
                LastName = value.LastName;
                ContactNumber = value.ContactNumber;
                Email = value.Email;
                Address = value.Address;
                DateCreated = value.DateCreated;
                IsActive = value.IsActive;

            }
        }

        [RelayCommand]
        private void ClearUserForm() => ResetInputFields();

        private void ResetInputFields()
        {
            Id = 0;
            FirstName = LastName = Address = ContactNumber = ContactNumber= Email = string.Empty;
           
            SelectedCustomer = null;
        }

        #endregion

        #region Customer Manipulation Add, Update/Edit and Delete
        [RelayCommand]
        public async Task AddCustomerAsync()
        {

            if (Customers.Any(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase)))
            {
                
                await ShowMessage("Duplicate Email", $"The email '{Email}' is already taken.");
                return;
            }

            try
            {
               
                var newCustomer = new Customer
                {

                    FirstName = FirstName,
                    LastName = LastName,
                    ContactNumber = ContactNumber,
                    Email = Email,
                    Address = Address,
                    DateCreated = DateTime.Now.Date,
                    IsActive = IsActive
                };

                filteredCustomers.Add(newCustomer);
                _dbManager.AddCustomer(newCustomer);
                loadCustomer();

                
                int lastPage = (int)Math.Ceiling((double)Customers.Count / PageSize);
                CurrentPage = lastPage;

                ResetInputFields();
                LoadPage(CurrentPage);
                await ShowMessage("Customer Created", $"New Customer: '{FirstName + LastName}' has been added on Page {CurrentPage}.");
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"An error occurred while adding the customer: {ex.Message}");
                return;
            }


            
        }

        [RelayCommand]
        public async Task RemoveCustomerAsync()
        {
            if (SelectedCustomer is null)
                return;
            
            var confirmBox = MessageBoxManager.GetMessageBoxStandard(
                "Confirm Delete",
                $"Are you sure you want to Delete Customer '{SelectedCustomer.FirstName + SelectedCustomer.LastName}'?",
                ButtonEnum.YesNo,
                Icon.Warning);

            var result = await confirmBox.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                
                try
                {
                    //_dbManager.RemoveCustomer(SelectedCustomer.Id);
                    _dbManager.DeleteCustomerWithSales(SelectedCustomer.Id);
                    Customers.Remove(SelectedCustomer);
                    ResetInputFields();
                    SelectedCustomer = null;

                    var infoBox = MessageBoxManager.GetMessageBoxStandard("Deleted", "Customer deleted successfully.", ButtonEnum.Ok);
                    await infoBox.ShowAsync();
                    loadCustomer();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while deleting: {ex.Message}");
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to delete Customer. {ex.Message}", ButtonEnum.Ok);
                    await errorBox.ShowAsync();
                }
            }
        }

        [RelayCommand]
        public async Task UpdateCustomerAsync()
        {
            if (SelectedCustomer is null) return;

            bool isEmailDuplicate = Customers.Any(u =>
                u.Id != SelectedCustomer.Id &&
                u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

            if (isEmailDuplicate)
            {
                await ShowMessage("Duplicate Email", $"The email '{Email}' is already taken.");
                return;
            }


            try
            {
               
                Debug.WriteLine($"Updating User: {Id}, {FirstName}, {LastName}, {Email}, {Address}, {ContactNumber}, {IsActive},");
                var updatedCustomer = new Customer
                {
                    Id = Id,
                    FirstName = FirstName,
                    LastName = LastName,
                    ContactNumber = ContactNumber,
                    Email = Email,
                    Address = Address,
                    DateCreated = DateCreated,
                    IsActive = IsActive
                };
                _dbManager.UpdateCustomer(updatedCustomer);

                var index = Customers.IndexOf(SelectedCustomer);
                if (index >= 0)
                {
                    Customers[index] = new Customer
                    {
                        Id = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        ContactNumber = ContactNumber,
                        Email = Email,
                        Address = Address,
                        DateCreated = DateCreated,
                        IsActive = IsActive
                    };
                }

                await ShowMessage("Updated", "User updated successfully.");
                ResetInputFields();
                loadCustomer();
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"Update failed: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task ViewPurchaseHistory(int customerId)
        {
            
            if (customerId == 0)
            {
                await ShowMessage("No Customer Id", "Please select a customer to view their purchase history.");
                return;
            }

            try
            {
                var vm = new PurchaseHistoryViewModel(customerId);
                var window = new PurchaseHistoryView
                {
                    DataContext = vm
                };

                window.Show();
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"Failed to load purchase history: {ex.Message}");
            }
        }

        #endregion

        #region Helper 
        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
        #endregion
    }
}
