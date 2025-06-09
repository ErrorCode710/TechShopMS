using CommunityToolkit.Mvvm.Input;
using TechShopMS.Models;
using TechShopMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.ViewModels
{
    public partial class PurchaseHistoryViewModel : ViewModelBase
    {
        public ObservableCollection<Sale> Sales { get; } = new();
        private readonly SaleDbManager _saleDbManager;

        public PurchaseHistoryViewModel(int customerId)
        {
            Debug.WriteLine("PURCHASE WINDOW IS RUNNING");
            _saleDbManager = new SaleDbManager("Data Source=data/SalesDB.db");
            // Load sales from your database here using the customerId
            var history = _saleDbManager.GetSalesByCustomer(customerId);

            foreach (var sale in history)
                Sales.Add(sale);
        }

        [RelayCommand]
        public void ViewSaleItemHistory(Sale selectedSale)
        {
            if (selectedSale is null || selectedSale.Items == null || !selectedSale.Items.Any())
                return;

            var vm = new SaleItemHistoryViewModel(selectedSale.Items);
            var window = new SaleItemHistoryView
            {
                DataContext = vm
            };
            window.Show();
             // optional: use Show() instead of ShowDialog() if non-blocking
        }
    }
}
