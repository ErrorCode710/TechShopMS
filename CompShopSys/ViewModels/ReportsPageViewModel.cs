using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class ReportsPageViewModel : ViewModelBase
    {

        // TODO:
        // ADD SORT ON THE STATUS STOCK
        // IF I WILL DELETE THE CUSTOMER DELETE ALSO THE HISTORY
        private readonly SaleDbManager _saleDbManager;
        private readonly ProductDbManager _productDbManager;
        private readonly CustomerDbManager _customerDbManager;

        [ObservableProperty] private ObservableCollection<SalesReport> salesReports;
        [ObservableProperty] private ObservableCollection<InventoryReport> inventoryReports;
        [ObservableProperty] private ObservableCollection<CustomerReport> customerReports;
        public int OutOfStockCount => InventoryReports.Count(i => i.Status == "Out of Stock");
        public int LowStockCount => InventoryReports.Count(i => i.Status == "Low Stock");
        public int InStockCount => InventoryReports.Count(i => i.Status == "In Stock");
        public decimal MonthlyTotalSales =>
      SalesReports
      .Where(x => x.SaleDate.Month == DateTime.Now.Month && x.SaleDate.Year == DateTime.Now.Year)
      .Sum(x => x.TotalAmount);

        public decimal DailyTotalSales =>
            SalesReports
            .Where(x => x.SaleDate.Date == DateTime.Today)
            .Sum(x => x.TotalAmount);

        public string FrequentCustomer =>
            SalesReports
            .GroupBy(x => x.CustomerName)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? "No Data";

        public ReportsPageViewModel()
        {
            _productDbManager = new ProductDbManager("Data Source=data/ProductDB.db");
            _customerDbManager = new CustomerDbManager("Data Source=data/CustomerDB.db");
            _saleDbManager = new SaleDbManager("Data Source=data/SalesDB.db");
            LoadReports();
        }

        private void LoadReports()
        {
            try
            {
                Debug.WriteLine("REPORTS PAGE IS RUNNING");
                SalesReports = new ObservableCollection<SalesReport>(_saleDbManager.GetSalesReport());
                InventoryReports = new ObservableCollection<InventoryReport>(_productDbManager.GetInventoryReport());

                var customers = _customerDbManager.GetAllCustomers(); // You must have this method in your CustomerDbManager
                var customerReportsList = new List<CustomerReport>();


                foreach (var customer in customers)
                {
                    var sales = _saleDbManager.GetSalesByCustomer(customer.Id);
                    if (sales.Any())
                    {
                        customerReportsList.Add(new CustomerReport
                        {
                            FullName = $"{customer.FirstName} {customer.LastName}",

                            TotalPurchases = sales.Count,
                            TotalSpent = sales.Sum(s => s.TotalAmount),
                            LastPurchaseDate = sales.Max(s => s.SaleDate)
                        });
                    }
                }

                CustomerReports = new ObservableCollection<CustomerReport>(customerReportsList);


                Debug.WriteLine($"Loaded {SalesReports.Count} sales reports and {InventoryReports.Count} inventory reports.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading reports: " + ex.Message);
            }
        }

        //public CustomerReport GenerateCustomerReport(int customerId, string fullName)
        //{
        //    var sales = _saleDbManager.GetSalesByCustomer(customerId);

        //    return new CustomerReport
        //    {
        //        FullName = fullName,
        //        TotalPurchases = sales.Count,
        //        TotalSpent = sales.Sum(s => s.TotalAmount),
        //        LastPurchaseDate = sales.Max(s => s.SaleDate)
        //    };
        //}

    }

}
