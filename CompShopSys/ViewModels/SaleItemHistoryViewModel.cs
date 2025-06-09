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
    public partial class SaleItemHistoryViewModel : ViewModelBase
    {
        public ObservableCollection<SaleItem> SaleItems { get; } = new();
        private readonly SaleDbManager _saleDbManager;

        public SaleItemHistoryViewModel(List<SaleItem> items)
        {
            foreach (var item in items)
                SaleItems.Add(item);
        }
    }
}
