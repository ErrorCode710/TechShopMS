using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechShopMS.Models;
using TechShopMS.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.ViewModels
{
    public partial class InventoryPageViewModel : ViewModelBase
    {
        private readonly ProductDbManager _dbManager;

        public InventoryPageViewModel()
        {
            _dbManager = new ProductDbManager("Data Source=data/ProductDB.db");
            LoadProducts();

            userRole = SessionService.CurrentRole;
            Debug.WriteLine($"UserRole on Inventory Page is {userRole}");

        }

        #region Observable Properties

        public string userRole { get; set;}


        [ObservableProperty] private ObservableCollection<Product> products = new();
        [ObservableProperty] private ObservableCollection<Product> filteredProducts = new();
        [ObservableProperty] private ObservableCollection<Product> pagedProducts = new();

        [ObservableProperty] private int id;
        [ObservableProperty] private string sku;
        [ObservableProperty] private string name = string.Empty;
        [ObservableProperty] private string category = string.Empty;
        [ObservableProperty] private string brand = string.Empty;
        [ObservableProperty] private decimal price;
        [ObservableProperty] private int quantity;
        [ObservableProperty] private string? imagePath = "Assets/Images/noImage.jpeg";
        [ObservableProperty] private string? description;

        [ObservableProperty] private string searchQuery = string.Empty;
        [ObservableProperty] private ObservableCollection<Product> filteredProduct = new(); // Not used?

        [ObservableProperty] private int currentPage = 1;
        [ObservableProperty] private int totalPages;


        [ObservableProperty]
        private Product selectedProduct;

        #endregion

        #region Search

        partial void OnSearchQueryChanged(string value) => Search();

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredProducts = new ObservableCollection<Product>(Products);
            }
            else
            {
                string lowerQuery = SearchQuery.Trim().ToLower();
                var filtered = Products.Where(item =>
                    (!string.IsNullOrEmpty(item.ProductName) && item.ProductName.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(item.Description) && item.Description.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(item.Brand) && item.Brand.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(item.Category) && item.Category.ToLower().Contains(lowerQuery))
                );

                FilteredProducts = new ObservableCollection<Product>(filtered);
            }

            CurrentPage = 1;
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            if (FilteredProducts == null || FilteredProducts.Count == 0)
            {
                PagedProducts = new ObservableCollection<Product>();
                TotalPages = 1;
                return;
            }

            TotalPages = (int)Math.Ceiling((double)FilteredProducts.Count / PageSize);
            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);

            var paged = FilteredProducts
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedProducts = new ObservableCollection<Product>(paged);
        }

        #endregion

        #region Pagination

        private const int PageSize = 5;

        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
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
            }
        }

        private void LoadProducts()
        {
            var allProducts = _dbManager.GetAllProducts();
            Products = new ObservableCollection<Product>(allProducts);
            FilteredProduct = new ObservableCollection<Product>(Products);
            TotalPages = (int)Math.Ceiling((double)Products.Count / PageSize);
            CurrentPage = 1;
            LoadPage(CurrentPage);
        }

        private void LoadPage(int pageNumber)
        {
            var pageItems = Products
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedProducts = new ObservableCollection<Product>(pageItems);
        }

        #endregion

        #region Product Manipulation

        [RelayCommand]
        public async Task AddProductAsync()
        {
            var validationErrors = GetValidationErrors();

            if (validationErrors.Length > 0)
            {
                await ShowMessage("Validation Error", validationErrors);
                return;
            }

            


            if (Products.Any(product => product.SKU.Equals(Sku, StringComparison.OrdinalIgnoreCase)))
            {
                await ShowMessage("Duplicate SKU", $"The SKU '{Sku}' is already taken.");
                return;
            }


            Debug.WriteLine("CLICKED!");
            if (string.IsNullOrEmpty(Sku) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Category) ||
                string.IsNullOrEmpty(Brand) || Price <= 0 || Quantity <= 0)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Please fill in all fields.", ButtonEnum.Ok);
                await box.ShowAsync();
                return;
            }

            var newProduct = new Product
            {
                SKU = Sku,
                ProductName = Name,
                Category = Category,
                Brand = Brand,
                Description = Description,
                ImagePath = string.IsNullOrWhiteSpace(ImagePath) ? "Assets/Images/noImage.jpeg" : ImagePath,
                Price = Price,
                Quantity = Quantity
            };

            FilteredProducts.Add(newProduct);
            _dbManager.AddProduct(newProduct);
            LoadProducts();

            int lastPage = (int)Math.Ceiling((double)Products.Count / PageSize);
            CurrentPage = lastPage;
            LoadPage(CurrentPage);

            var box1 = MessageBoxManager.GetMessageBoxStandard("Success", $"New Product Added on Page {CurrentPage}", ButtonEnum.Ok);
            await box1.ShowAsync();
        }
        // ERROR: WHEN DELETE IT CATCH THE ERROR OBJECCT INTERFERENCE
        [RelayCommand]
        private async Task DeleteProductAsync() {
            if(userRole == "Staff")
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Access Denied", "Only Admin or Manager can delete a product.", ButtonEnum.Ok, Icon.Warning);
                await box.ShowAsync();
                return;
            }
            if (SelectedProduct is null)
                return;

            var confirmBox = MessageBoxManager.GetMessageBoxStandard(
                "Confirm Remove",
                $"Are you sure you want to Remove Product '{SelectedProduct.ProductName}'?",
                ButtonEnum.YesNo,
                Icon.Warning);

            var result = await confirmBox.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                try
                {
                    _dbManager.RemoveProduct(SelectedProduct.Id);
                    Products.Remove(SelectedProduct);

                    var infoBox = MessageBoxManager.GetMessageBoxStandard("Removed", "Product Removed successfully.", ButtonEnum.Ok);
                    await infoBox.ShowAsync();



                    ResetInputFields();
                    




                    LoadProducts();
                    int lastPage = (int)Math.Ceiling((double)Products.Count / PageSize);
                    
                    LoadPage(CurrentPage);


                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while deleting: {ex.Message}");
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to Remove Product. {ex.Message}", ButtonEnum.Ok);
                    await errorBox.ShowAsync();
                }
            }

        }
        [RelayCommand]
        private void ClearUserForm(){ 
            ResetInputFields();
            //_dbManager.SeedProducts();
            //_dbManager.SeedProductsWithImage();

        }         

        private void ResetInputFields()
        {
            Id = 0;
            Sku = Brand = Category = Name = Description = ImagePath = string.Empty;
            Price = 0;
            Quantity = 0;

            //SelectedProduct = null;

        }

        [RelayCommand]
        private async Task EditProductAsync()
        {
            if (userRole == "Staff")
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Access Denied", "Only Admin or Manager can Edit/Update a product.", ButtonEnum.Ok, Icon.Warning);
                await box.ShowAsync();
                return;
            }
            if (SelectedProduct is null) return;
            var validationErrors = GetValidationErrors();

            if (validationErrors.Length > 0)
            {
                await ShowMessage("Validation Error", validationErrors);
                return;
            }

            if (Products.Any(product =>
         product.SKU.Equals(Sku, StringComparison.OrdinalIgnoreCase) &&
         product.Id != Id)) 
            {
                await ShowMessage("Duplicate SKU", $"The SKU '{Sku}' is already taken.");
                return;
            }


            Debug.WriteLine("CLICKED!");
            if (string.IsNullOrEmpty(Sku) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Category) ||
                string.IsNullOrEmpty(Brand) || Price <= 0 || Quantity <= 0)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Please fill in all fields.", ButtonEnum.Ok);
                await box.ShowAsync();
                return;
            }
            try
            {
                //int numericRole = (int)Role;
                _dbManager.UpdateProduct(Id, Sku, Name, Category, Brand, Description,
                string.IsNullOrWhiteSpace(ImagePath) ? "Assets/Images/noImage.jpeg" : ImagePath,
                Price, Quantity);
                Debug.WriteLine($"Product Updated: {Sku}, {Name}");
                Debug.WriteLine($"Updating product with ID: {Id}");

                var index = Products.IndexOf(selectedProduct);
                if (index >= 0)
                {
                    Products[index] = new Product
                    {
                        Id = Id,
                        SKU = Sku,
                        ProductName = Name,
                        Category = Category,
                        Brand = Brand,
                        Description = Description,
                        ImagePath = ImagePath,
                        Price = Price,
                        Quantity = Quantity
                    };

                }

                LoadProducts();
                await ShowMessage("Updated", "Product updated successfully.");
                ResetInputFields();
                
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"Update failed: {ex.Message}");
            }
        }

        #endregion

        #region Image Handling

        [RelayCommand]
        public async Task AddPictureAsync(Control view)
        {
            var imagePath = await SelectAndCopyImageAsync(view);
            if (!string.IsNullOrEmpty(imagePath))
            {
                ImagePath = imagePath;
                Debug.WriteLine($"Image copied to: {ImagePath}");
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Notice", "No image selected", ButtonEnum.Ok);
                await box.ShowAsync();
            }
        }

        public async Task<string?> SelectAndCopyImageAsync(Control view)
        {
            var topLevel = view.GetVisualRoot() as TopLevel;

            if (topLevel?.StorageProvider is not { CanOpen: true } storage)
            {
                Debug.WriteLine("StorageProvider not available.");
                return null;
            }

            var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Product Image",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Image Files")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif", "*.webp" }
                    }
                }
            });

            if (files.Count == 0)
                return null;

            var originalPath = files[0].Path.LocalPath;
            return CopyImageToAssets(originalPath);
        }

        public string CopyImageToAssets(string originalFilePath)
        {
            string fileName = Path.GetFileName(originalFilePath);
            string assetsFolder = Path.Combine(AppContext.BaseDirectory, "Assets", "Images");
            Directory.CreateDirectory(assetsFolder);

            string destPath = Path.Combine(assetsFolder, fileName);
            if (!File.Exists(destPath))
                File.Copy(originalFilePath, destPath);

            return $"Assets/Images/{fileName}";
        }

        #endregion

        #region Input Validation
        
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Sku) && string.IsNullOrWhiteSpace(Sku))
                    return "Proudct Name cannot be empty.";
                if (columnName == nameof(Brand) && string.IsNullOrWhiteSpace(Brand))
                    return "Brand Name cannot be empty.";
                if (columnName == nameof(Category) && string.IsNullOrWhiteSpace(Category))
                    return "Category cannot be empty.";
                if (columnName == nameof(Price) && string.IsNullOrWhiteSpace(Price.ToString()))
                    return "Price cannot be ₱0.";
                
                return string.Empty;
            }
        }
        #endregion

        public string Error => "Please ensure all fields are filled correctly.";

        private string GetValidationErrors()
        {
            var sb = new StringBuilder();
            foreach (var prop in new[] { nameof(Name), nameof(Sku), nameof(Quantity), nameof(Price), nameof(Category) })
            {
                var error = this[prop];
                if (!string.IsNullOrWhiteSpace(error))
                    sb.AppendLine(error);
            }
            return sb.ToString();
        }

        #region Helpers

        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
        partial void OnSelectedProductChanged(Product value)
        {
            if (value is not null)
            {
                Id = value.Id;
                Sku = value.SKU;
                Name = value.ProductName;
                Category = value.Category;
                Brand = value.Brand;
                Description = value.Description;
                ImagePath = value.ImagePath;
                Price = value.Price;
                Quantity = value.Quantity;
            }

            Debug.WriteLine(value.ImagePath);
        }
        #endregion
    }
}
