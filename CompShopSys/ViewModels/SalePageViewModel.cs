using CommunityToolkit.Mvvm.ComponentModel;
using CompShopSys.Models;
using CompShopSys.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using CommunityToolkit.Mvvm.Input;
using Tmds.DBus.Protocol;


namespace CompShopSys.ViewModels
{
    public partial class SalePageViewModel : ViewModelBase
    {
        #region Fields
        //private readonly CustomerDbManager _dbManager;
        private const int PageSize = 3;

        public List<bool> BooleanValues { get; set; } = new List<bool>() { true, false };
        public bool SelectedBoolean { get; set; }

        private readonly ProductDbManager _productDbManager;
        private readonly CustomerDbManager _customerDbManager;
        private readonly SaleDbManager _saleDbManager ;
        public string IssuedOnDisplay => issuedOn.ToString("MMM dd, yyyy - hh:mm tt");
        // FOR INVOICE GEN
        private readonly InvoiceGenService _invoiceService = new();

        private int _saleItemIdCounter = 1;

        private Dictionary<string, int> _originalStocks = new();
        private Dictionary<string, int> _reservedStocks = new();
        #endregion

        #region Observable Properties

       
        // For Sale Interface
        [ObservableProperty] private int id;
        [ObservableProperty] private string invoiceNumber ;
        //[ObservableProperty] private decimal subTotal;
        //[ObservableProperty] private decimal discount;
        //[ObservableProperty] private decimal paymentMethod;
        [ObservableProperty] private decimal amountPaid;
        [ObservableProperty] private decimal totalAmount;
        [ObservableProperty] private decimal change;
        [ObservableProperty] private DateTime saleDate;
        [ObservableProperty] private int totalQuantity;




        // For Item Input Form
        [ObservableProperty] private string productName;
        [ObservableProperty] private int productId;
        [ObservableProperty] private string brand ;
        [ObservableProperty] private string category;
        [ObservableProperty] private int quantity;
        public int AvailableQuantity =>
    SelectedProduct != null
        ? GetAvailableStock(SelectedProduct)  // Uses the reservation logic
        : 0;
        [ObservableProperty] private decimal price;
        [ObservableProperty] private int stockQuantity;

        // For Search First Product Interface
        [ObservableProperty] private ObservableCollection<Product> allProducts = new();
        [ObservableProperty] private ObservableCollection<Product> filteredProducts = new();
        [ObservableProperty] private string searchProductText;
        [ObservableProperty] private Product selectedProduct;

        //For DataGrid ItemSource
        [ObservableProperty] private ObservableCollection<SaleItem> saleItems = new(); // item sourced 
       

        [ObservableProperty] private SaleItem selectedSaleItem;

        // For Customer Search 
        [ObservableProperty] private ObservableCollection<Customer> allCustomers = new();
        [ObservableProperty] private ObservableCollection<Customer> filteredCustomers = new();
        [ObservableProperty] private string searchCustomerText;
        [ObservableProperty] private Customer selectedCustomer;

        // For Invoice Display
        
        [ObservableProperty] private string customerName;
        [ObservableProperty] private string customerEmail;
        [ObservableProperty] private string customerPhoneNo;
        [ObservableProperty] private string issueBy;

        [ObservableProperty]private DateTime issuedOn = DateTime.Now;


        // For Pagination
        [ObservableProperty] private ObservableCollection<SaleItem> filteredSaleItems = new();
        [ObservableProperty] private ObservableCollection<SaleItem> pagedSaleItem = new();
        [ObservableProperty] private string searchQuery;
        [ObservableProperty] private int currentPage = 1;
        [ObservableProperty] private int totalPages;


        #endregion

        #region Constructor
        // TODO:
        // PUT PAGINATION on DataGrid (DONE)
        // NEED TO REFLECT ON THE PRODUCTS LIKE MINUS STOCK
        // STORE THE SALEITEM ON THE SALE TO DATABASE (DONE)
        // AFTER COMPLETE SALE EMPTY THE INPUTS AND THE CUSTOMER DETAILS (DONE)
        // PURCHASE HISTORY AMOUNT AND CHANGE NEED TO BE IN PHP STRING FORMAT (DONE)
        // REMOVE ITEM FROM THE ITEM FORM  (Done)
        // UPDATE ITEM FROM THE ITEM FORM (Done )
        // CLEAR INPUT BUTTON  (DONe)

        public SalePageViewModel()
        {
            

            Debug.WriteLine("[SalePageViewModel] Initializing SalePageViewModel");
            IssuedOn = DateTime.Now;

            _productDbManager = new ProductDbManager("Data Source=data/ProductDB.db");
            _customerDbManager = new CustomerDbManager("Data Source=data/CustomerDB.db");
            _saleDbManager = new SaleDbManager("Data Source=data/SalesDB.db");


            Debug.WriteLine($"Generated Invoice Number: {InvoiceNumber}");
            InitializeAsync();

            foreach (var customer in AllCustomers)
            {
                Debug.WriteLine($"Customer First Name: {customer.FirstName}");
            }

            SaleItems.CollectionChanged += (s, e) => saleItemPopulate();



        }
        #endregion

        #region Pagination
        public void saleItemPopulate()
        {
            FilteredSaleItems = new ObservableCollection<SaleItem>(SaleItems);
            TotalPages = (int)Math.Ceiling((double)FilteredSaleItems.Count / PageSize);
            CurrentPage = 1;
            LoadPage(CurrentPage);
        }
        private void LoadPage(int pageNumber)
        {
            var pageItems = SaleItems
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedSaleItem = new ObservableCollection<SaleItem>(pageItems);


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
        #region Load 

        public async Task LoadProducts()
        {
            try
            {
                var products = await Task.Run(() => _productDbManager.GetAllProducts());
                AllProducts = new ObservableCollection<Product>(products);
                FilteredProducts = new ObservableCollection<Product>(products);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading products: {ex.Message}");
            }
        }
        public async Task LoadCustomers()
        {
            try
            {
                var customers = await Task.Run(() => _customerDbManager.GetAllCustomers());
                AllCustomers = new ObservableCollection<Customer>(customers);
                FilteredCustomers = new ObservableCollection<Customer>(customers);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading customer: {ex.Message}");
            }

        }

        private async void InitializeAsync()
        {
            try
            {
                await LoadProducts();
                await LoadCustomers();
                Debug.WriteLine(" loaded successfully");
            }
            catch (Exception ex)
            {
                // Show user-friendly error
                await ShowMessage("Failed to load : ", ex.Message);
            }
        }
        partial void OnSelectedProductChanged(Product? value)
        {
            if (value is null) return;
            OnPropertyChanged(nameof(AvailableQuantity));
            // Example: Autofill other fields (if you're not using direct binding)
            Brand = value.Brand;
            Category = value.Category;
            Price = value.Price;
            StockQuantity = value.Quantity;
            // etc.
        }
        partial void OnSelectedCustomerChanged(Customer? value)
        {
            if (value is null) return;
            

            CustomerName = $"{value.FirstName} {value.LastName}";
            CustomerEmail = value.Email;
            CustomerPhoneNo = value.ContactNumber;
            IssueBy = $"{SessionService.CurrentRole } {SessionService.CurrentUser}";
            IssuedOn = DateTime.Now;

            



        }
        partial void OnQuantityChanged(int value)
        {

            if (SelectedProduct is null)
                return;

            if (value > AvailableQuantity)
            {
                Quantity = AvailableQuantity;
                ShowMessage("Alert", $"Only {AvailableQuantity} available");
            }
        }

        
        partial void OnAmountPaidChanged(decimal value)
        {
            CalculateChange();
        }

        #endregion
        #region Add, Edit, Delete
        [RelayCommand]
        public async Task AddSaleItem()
        {
            if (SelectedProduct is null)
            {
                await ShowMessage("Error", "Please select a product.");
                return;
            }
            if (SelectedCustomer is null)
            {
                await ShowMessage("Error", "Please select a customer.");
                return;
            }
            int availableStock = GetAvailableStock(SelectedProduct);

            if (Quantity <= 0 || Quantity > availableStock)
            {
                await ShowMessage("Error", $"Only {availableStock} units available");
                return;
            }

            if (!_originalStocks.ContainsKey(SelectedProduct.SKU))
            {
                _originalStocks[SelectedProduct.SKU] = SelectedProduct.Quantity;
            }

            // Reserve the stock
            _reservedStocks[SelectedProduct.SKU] =
                (_reservedStocks.TryGetValue(SelectedProduct.SKU, out var current) ? current : 0) + Quantity;
            if (Quantity <= 0 || Quantity > availableStock)
            {
                await ShowMessage("Error", $"Only {availableStock} units available");
                return;
            }
            var saleItem = new SaleItem
            {
                Id = _saleItemIdCounter++, // Simple ID generation, consider using a better approach
                SKU = SelectedProduct.SKU,
                ProductName = SelectedProduct.ProductName,
                Quantity = Quantity,
                UnitPrice = SelectedProduct.Price,
                Description = SelectedProduct.Description,
                
                
            };

           
            SaleItems.Add(saleItem);
            CalculateTotalAmount();
            CalculateTotalQuantity();
            //saleItemPopulate();

            //LogSaleItems();

        }
        [RelayCommand]
        public async Task RemoveSaleItem()
        {
            if (SelectedSaleItem is null)
            {
                await ShowMessage("Error", "Please select an item to remove.");
                return;
            }

            // Release reserved stock
            if (_reservedStocks.ContainsKey(SelectedSaleItem.SKU))
            {
                _reservedStocks[SelectedSaleItem.SKU] -= SelectedSaleItem.Quantity;
                if (_reservedStocks[SelectedSaleItem.SKU] <= 0)
                    _reservedStocks.Remove(SelectedSaleItem.SKU);
            }

            var itemToRemove = SaleItems.FirstOrDefault(item => item.SKU == SelectedSaleItem.SKU);
            if (itemToRemove != null)
            {
                SaleItems.Remove(itemToRemove);
                CalculateTotalAmount();
                CalculateTotalQuantity();
                SelectedSaleItem = null;
            }
        }

        [RelayCommand]
        public async Task UpdateSaleItem()
        {
            if (SelectedSaleItem is null)
            {
                await ShowMessage("Error", "No item selected");
                return;
            }

            // Directly update quantity from bound property
            if (Quantity <= 0 || Quantity > SelectedProduct.Quantity)
            {
                await ShowMessage("Error", "Invalid quantity");
                return;
            }

            var itemToUpdate = SaleItems.FirstOrDefault(item => item.SKU == SelectedSaleItem.SKU);
            if (itemToUpdate == null)
            {
                await ShowMessage("Error", "Selected item not found in current sale.");
                return;
            }
            if (Quantity <= 0 || Quantity > SelectedProduct.Quantity)
            {
                await ShowMessage("Error", $"Quantity must be between 1 and {SelectedProduct.Quantity}");
                return;
            }

            itemToUpdate.Quantity = Quantity;

            CalculateTotalAmount();
            CalculateTotalQuantity();
            saleItemPopulate();
            await ShowMessage("Sucess", "Edit Successfully");
        }

        [RelayCommand]
        private void ClearUserForm() => ClearSaleItemForm();

        [RelayCommand]
        public async Task CompleteSale()
        {
            if(selectedCustomer is null)
            {
                await ShowMessage("Error", "Please select a customer.");
                return;
            }
            if (SaleItems.Count == 0 )
            {
                await ShowMessage("Error", "Add Atleast One Item.");
                return;
            }

            if (AmountPaid <= 0)
            {
                await ShowMessage("Error", "Please enter a valid amount paid.");
                return;
            }
            if (AmountPaid < TotalAmount)
            {
                await ShowMessage("Error", "Amount paid is less than total amount.");
                return;
            }
            try
            {
                // Update ACTUAL database stock
                foreach (var item in SaleItems)
                {
                    await _productDbManager.UpdateProductStockAsync(item.SKU, -item.Quantity);
                }

                // Save sale
                InvoiceNumber = _invoiceService.GenerateInvoiceNumber();
                var newSale = new Sale
                {
                    CustomerId = SelectedCustomer.Id,
                    InvoiceNumber = InvoiceNumber,
                    SaleDate = IssuedOn,
                    TotalAmount = TotalAmount,
                    AmountPaid = AmountPaid,
                    Change = Change,
                    Items = SaleItems.ToList()
                };

                _saleDbManager.AddSale(newSale);

                
                _originalStocks.Clear();
                _reservedStocks.Clear();
                LoadProducts();
                ClearSaleForm();
                await ShowMessage("Success", "Sale completed successfully!");
            }
            catch (Exception ex)
            {
                // Rollback reservations if DB fails
                _reservedStocks.Clear();
                await ShowMessage("Error", $"Sale failed: {ex.Message}");
            }

        }
        partial void OnIssuedOnChanged(DateTime value)
        {
            OnPropertyChanged(nameof(IssuedOnDisplay));
        }

        partial void OnSelectedSaleItemChanged(SaleItem? value)
        {
            if (value is not null)
            {
                // This is now 100% safe and accurate!
                var originalProduct = AllProducts.FirstOrDefault(p => p.SKU == value.SKU);

                if (originalProduct is not null)
                {
                    SelectedProduct = originalProduct;
                }

                Id = value.Id;
                ProductName = value.ProductName;
                Quantity = value.Quantity;
                Price = value.UnitPrice;
            }
        }
        #endregion

        #region Helper Methods

        private void CalculateTotalAmount()
        {
            TotalAmount = SaleItems?.Sum(item => item.Total) ?? 0;
            

            Debug.WriteLine($"Total Amount: ₱{TotalAmount:F2}");
            OnPropertyChanged(nameof(TotalAmount));
            
        }
        private void CalculateChange()
        {
            Change = Math.Max(AmountPaid - TotalAmount, 0);
            OnPropertyChanged(nameof(Change));
        }
        private void CalculateTotalQuantity()
        {
            TotalQuantity = SaleItems?.Sum(item => item.Quantity) ?? 0;
        }

        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
        public void RefreshIssuedOn()
        {
            IssuedOn = DateTime.Now;
            OnPropertyChanged(nameof(IssuedOnDisplay));
        }

        private void ClearSaleForm()
        {
            SelectedCustomer = null;
            InvoiceNumber = string.Empty;
            IssuedOn = DateTime.Now; // or default(DateTime)
            TotalAmount = 0;
            AmountPaid = 0;
            Change = 0;

            TotalQuantity = 0;
            CustomerName = string.Empty;
            CustomerEmail = string.Empty;
            CustomerPhoneNo = string.Empty;
            SaleItems.Clear();
            ClearSaleItemForm();
        }

        private void ClearSaleItemForm()
        {
            SelectedProduct = null;
            ProductName = string.Empty;
            Quantity = 1; // Reset to default value
            Price = 0;
            StockQuantity = 0;
            Brand = string.Empty;
            Category = string.Empty;
            SelectedSaleItem = null; // Clear selected item
        }
        public void LogSaleItems()
        {
            if (saleItems == null || saleItems.Count == 0)
            {
                Debug.WriteLine("SaleItems list is empty.");
                return;
            }

            Debug.WriteLine("=== SaleItems Log ===");
            foreach (var item in saleItems)
            {
                Debug.WriteLine($"ID: {item.Id}");
                Debug.WriteLine($"Product: {item.ProductName}");
                Debug.WriteLine($"Description: {item.Description}");
                Debug.WriteLine($"Quantity: {item.Quantity}");
                Debug.WriteLine($"Unit Price: ₱{item.UnitPrice:F2}");
                Debug.WriteLine($"Total: ₱{item.Total:F2}");
                Debug.WriteLine($"Discount: ₱{item.Discount:F2}");
                Debug.WriteLine("-----------------------");
            }
        }

        private int GetAvailableStock(Product product)
        {
            if (!_reservedStocks.ContainsKey(product.SKU))
                return product.Quantity;

            return product.Quantity - _reservedStocks[product.SKU];
        }
        #endregion
    }





}