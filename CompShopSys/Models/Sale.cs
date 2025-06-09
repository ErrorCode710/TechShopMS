using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopMS.Models;

namespace TechShopMS.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string InvoiceNumber { get; set; }
    
        public DateTime SaleDate { get; set; }

        public List<SaleItem> Items { get; set; } = new();  

        //public decimal SubTotal { get; set; }
        //public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Change { get; set; }
    }
}
