using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompShopSys.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        //public int Sku { get; set; }  // FK to 
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
        public decimal Discount { get; set; } = 0.0m; // Default to 0 if not specified

        public string Description { get; set; } = "No Description";
    }

}
