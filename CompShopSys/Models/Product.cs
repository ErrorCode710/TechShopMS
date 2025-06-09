using Avalonia.Controls.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Avalonia.Media.Imaging;
using CompShopSys.Helper;
using Avalonia.Platform;
using Path = System.IO.Path;

namespace CompShopSys.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        //public object Sku { get; internal set; }
        public string ProductName { get; set; }            
        public string Category { get; set; }         
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ImageResourcePath => $"avares://CompShopSys/Assets/Images/{ImagePath}";

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Bitmap? ImageUri
        {
            get
            {
                var fullPath = Path.Combine(
                    AppContext.BaseDirectory,
                    ImagePath?.Replace('/', Path.DirectorySeparatorChar)
                );

                Debug.WriteLine($"[ImageUri] Full Path: {fullPath}");

                // Check if file exists to avoid exceptions
                if (File.Exists(fullPath))
                {
                    return new Bitmap(fullPath);
                }
                else
                {
                    Debug.WriteLine($"[ImageUri] File not found: {fullPath}");
                    return null;
                }
            }
        }

        public override string ToString()
        {
            return ProductName; // or $"{ProductName} ({SKU})"
        }


    }
}
