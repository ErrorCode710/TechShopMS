using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.Models
{
    public class SalesReport
    {
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class InventoryReport
    {
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status => Quantity == 0 ? "Out of Stock"
                        : Quantity <= 5 ? "Low Stock"
                        : "In Stock";
    }
    public class CustomerReport
    {
        public string FullName { get; set; }
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastPurchaseDate { get; set; }
    }
}
