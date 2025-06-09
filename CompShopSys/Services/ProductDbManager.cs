using CompShopSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Tmds.DBus.Protocol;
using System.Diagnostics;

namespace CompShopSys.Services
{
    public class ProductDbManager : BaseDbManager
    {
        public ProductDbManager(string connectionString) : base(connectionString) { }

        public List<Product> GetAllProducts()
        {
            Connect();
            try
            {
                Debug.WriteLine("GetAllProducts is Running");
                var products = new List<Product>();
                string query = "SELECT * FROM Products";
                using var command = new SqliteCommand(query, _connection);
                using var reader = command.ExecuteReader();
              
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetInt32(0),
                        SKU = reader.GetString(1),
                        ProductName = reader.GetString(2),
                        Category = reader.GetString(3),
                        Brand = reader.GetString(4),
                        Description = reader.GetString(5),
                        ImagePath = reader.GetString(6),
                        Price = reader.GetDecimal(7),
                        Quantity = reader.GetInt32(8)
                    });
                }
                return products;
            }
            finally
            {
                Disconnect();
            }
        }
        public void AddProduct(Product product)
        {
            Connect();
            try
            {
                string query = "INSERT INTO Products (SKU, ProductName, Category, Brand, Description, ImagePath, Price, Quantity) " +
                               "VALUES (@SKU, @ProductName, @Category, @Brand, @Description, @ImagePath, @Price, @Quantity)";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@SKU", product.SKU);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Category", product.Category);
                command.Parameters.AddWithValue("@Brand", product.Brand);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }
        public void SeedProducts()
        {
            var products = new List<Product>
    {

        // Monitors
        new Product { SKU = "MN-AS001", ProductName = "ASUS TUF Gaming (144Hz)", Category = "Monitors", Brand = "ASUS", Description = "24\" FHD, 144Hz, 1ms", ImagePath = "", Price = 12000, Quantity = 8 },
        new Product { SKU = "MN-AC002", ProductName = "Acer Nitro (IPS Panel)", Category = "Monitors", Brand = "Acer", Description = "23.8\" FHD, IPS, 75Hz", ImagePath = "", Price = 8500, Quantity = 10 },
        new Product { SKU = "MN-SS003", ProductName = "Samsung Odyssey (Curved)", Category = "Monitors", Brand = "Samsung", Description = "27\" QHD, 144Hz, Curved", ImagePath = "", Price = 18000, Quantity = 5 },
        new Product { SKU = "MN-LG004", ProductName = "LG UltraGear (4K)", Category = "Monitors", Brand = "LG", Description = "27\" 4K, HDR, 144Hz", ImagePath = "", Price = 25000, Quantity = 4 },
        new Product { SKU = "MN-DL005", ProductName = "Dell Ultrasharp (Office)", Category = "Monitors", Brand = "Dell", Description = "24\" FHD, IPS, 60Hz", ImagePath = "", Price = 10000, Quantity = 7 },
        new Product { SKU = "MN-MS006", ProductName = "MSI Optix (Gaming)", Category = "Monitors", Brand = "MSI", Description = "27\" FHD, 165Hz, 1ms", ImagePath = "", Price = 15000, Quantity = 6 },
        new Product { SKU = "MN-VS007", ProductName = "ViewSonic (Budget)", Category = "Monitors", Brand = "ViewSonic", Description = "21.5\" FHD, 60Hz", ImagePath = "", Price = 6500, Quantity = 12 },
        new Product { SKU = "MN-BQ008", ProductName = "BenQ (Designer)", Category = "Monitors", Brand = "BenQ", Description = "24\" FHD, IPS, 99% sRGB", ImagePath = "", Price = 14000, Quantity = 5 },
        new Product { SKU = "MN-PH009", ProductName = "Philips (EyeCare)", Category = "Monitors", Brand = "Philips", Description = "22\" FHD, Low Blue Light", ImagePath = "", Price = 7500, Quantity = 9 },
        new Product { SKU = "MN-HP010", ProductName = "HP Pavilion (Basic)", Category = "Monitors", Brand = "HP", Description = "21.5\" FHD, 60Hz", ImagePath = "", Price = 7000, Quantity = 8 },

        // Keyboards
        new Product { SKU = "KB-RD001", ProductName = "Redragon K552 (Mechanical)", Category = "Keyboards", Brand = "Redragon", Description = "Mechanical, RGB, Blue Switches", ImagePath = "", Price = 2500, Quantity = 15 },
        new Product { SKU = "KB-RZ002", ProductName = "Razer BlackWidow (RGB)", Category = "Keyboards", Brand = "Razer", Description = "Mechanical, Chroma RGB", ImagePath = "", Price = 6000, Quantity = 8 },
        new Product { SKU = "KB-LG003", ProductName = "Logitech K120 (Membrane)", Category = "Keyboards", Brand = "Logitech", Description = "Membrane, Spill-Resistant", ImagePath = "", Price = 800, Quantity = 20 },
        new Product { SKU = "KB-RK004", ProductName = "Royal Kludge RK61 (Wireless)", Category = "Keyboards", Brand = "Royal Kludge", Description = "60% Wireless, Mechanical", ImagePath = "", Price = 3500, Quantity = 10 },
        new Product { SKU = "KB-CR005", ProductName = "Corsair K70 (Gaming)", Category = "Keyboards", Brand = "Corsair", Description = "Mechanical, RGB, Aluminum Frame", ImagePath = "", Price = 7000, Quantity = 6 },
        new Product { SKU = "KB-HX006", ProductName = "HyperX Alloy FPS", Category = "Keyboards", Brand = "HyperX", Description = "Mechanical, Red Switches", ImagePath = "", Price = 4500, Quantity = 9 },
        new Product { SKU = "KB-ST007", ProductName = "SteelSeries Apex", Category = "Keyboards", Brand = "SteelSeries", Description = "Hybrid Mechanical, RGB", ImagePath = "", Price = 5500, Quantity = 7 },
        new Product { SKU = "KB-RC008", ProductName = "Roccat Vulcan", Category = "Keyboards", Brand = "Roccat", Description = "Mechanical, RGB, Low-Profile", ImagePath = "", Price = 6500, Quantity = 4 },
        new Product { SKU = "KB-CM009", ProductName = "Cooler Master CK550", Category = "Keyboards", Brand = "Cooler Master", Description = "Mechanical, RGB, Gateron Switches", ImagePath = "", Price = 4000, Quantity = 8 },
        new Product { SKU = "KB-AP010", ProductName = "Apple Magic Keyboard", Category = "Keyboards", Brand = "Apple", Description = "Wireless, Slim Design", ImagePath = "", Price = 5000, Quantity = 5 },

        // Mice
        new Product { SKU = "MS-LG001", ProductName = "Logitech G102 (Budget Gaming)", Category = "Mice", Brand = "Logitech", Description = "Optical, RGB, 8K DPI", ImagePath = "", Price = 1200, Quantity = 20 },
        new Product { SKU = "MS-RZ002", ProductName = "Razer DeathAdder (Ergonomic)", Category = "Mice", Brand = "Razer", Description = "Ergonomic, 16K DPI", ImagePath = "", Price = 3000, Quantity = 12 },
        new Product { SKU = "MS-ST003", ProductName = "SteelSeries Rival 3", Category = "Mice", Brand = "SteelSeries", Description = "Lightweight, 8.5K DPI", ImagePath = "", Price = 2000, Quantity = 15 },
        new Product { SKU = "MS-RD004", ProductName = "Redragon M711 (RGB)", Category = "Mice", Brand = "Redragon", Description = "RGB, 10K DPI, 7 Buttons", ImagePath = "", Price = 1500, Quantity = 18 },
        new Product { SKU = "MS-CR005", ProductName = "Corsair Harpoon", Category = "Mice", Brand = "Corsair", Description = "Wireless, 10K DPI", ImagePath = "", Price = 2500, Quantity = 10 },
        new Product { SKU = "MS-AS006", ProductName = "ASUS ROG Gladius", Category = "Mice", Brand = "ASUS", Description = "RGB, 12K DPI, Swappable Switches", ImagePath = "", Price = 3500, Quantity = 8 },
        new Product { SKU = "MS-HX007", ProductName = "HyperX Pulsefire", Category = "Mice", Brand = "HyperX", Description = "RGB, 16K DPI, Lightweight", ImagePath = "", Price = 2800, Quantity = 9 },
        new Product { SKU = "MS-MS008", ProductName = "Microsoft Classic", Category = "Mice", Brand = "Microsoft", Description = "Wired, Basic Office Mouse", ImagePath = "", Price = 600, Quantity = 25 },
        new Product { SKU = "MS-HP009", ProductName = "HP X1000 (Basic)", Category = "Mice", Brand = "HP", Description = "Wired, 1.6K DPI", ImagePath = "", Price = 500, Quantity = 30 },
        new Product { SKU = "MS-AP010", ProductName = "Apple Magic Mouse", Category = "Mice", Brand = "Apple", Description = "Wireless, Multi-Touch", ImagePath = "", Price = 4000, Quantity = 6 },

        // Storage
        new Product { SKU = "ST-SG001", ProductName = "Seagate Barracuda 1TB HDD", Category = "Storage", Brand = "Seagate", Description = "3.5\", 7200 RPM", ImagePath = "", Price = 2500, Quantity = 15 },
        new Product { SKU = "ST-WD002", ProductName = "WD Blue 2TB HDD", Category = "Storage", Brand = "WD", Description = "3.5\", 5400 RPM", ImagePath = "", Price = 3500, Quantity = 12 },
        new Product { SKU = "ST-SS003", ProductName = "Samsung 870 EVO (SATA SSD)", Category = "Storage", Brand = "Samsung", Description = "500GB, SATA III", ImagePath = "", Price = 3000, Quantity = 10 },
        new Product { SKU = "ST-KN004", ProductName = "Kingston A400 (120GB SSD)", Category = "Storage", Brand = "Kingston", Description = "120GB, SATA III", ImagePath = "", Price = 1200, Quantity = 20 },
        new Product { SKU = "ST-CR005", ProductName = "Crucial MX500 (500GB SSD)", Category = "Storage", Brand = "Crucial", Description = "500GB, SATA III", ImagePath = "", Price = 2800, Quantity = 8 },
        new Product { SKU = "ST-WD006", ProductName = "WD Black SN750 (NVMe)", Category = "Storage", Brand = "WD", Description = "500GB, NVMe PCIe", ImagePath = "", Price = 4000, Quantity = 6 },
        new Product { SKU = "ST-SD007", ProductName = "SanDisk Ultra 3D SSD", Category = "Storage", Brand = "SanDisk", Description = "1TB, SATA III", ImagePath = "", Price = 5000, Quantity = 5 },
        new Product { SKU = "ST-TS008", ProductName = "Toshiba P300 HDD", Category = "Storage", Brand = "Toshiba", Description = "1TB, 7200 RPM", ImagePath = "", Price = 2300, Quantity = 10 },
        new Product { SKU = "ST-AD009", ProductName = "ADATA SU800 SSD", Category = "Storage", Brand = "ADATA", Description = "256GB, SATA III", ImagePath = "", Price = 1800, Quantity = 12 },
        new Product { SKU = "ST-SS010", ProductName = "Samsung 980 Pro (PCIe 4.0)", Category = "Storage", Brand = "Samsung", Description = "1TB, NVMe PCIe 4.0", ImagePath = "", Price = 7000, Quantity = 4 },

        // RAM
        new Product { SKU = "RM-KN001", ProductName = "Kingston Fury 8GB DDR4", Category = "RAM", Brand = "Kingston", Description = "8GB, 3200MHz, CL16", ImagePath = "", Price = 1800, Quantity = 20 },
        new Product { SKU = "RM-CR002", ProductName = "Corsair Vengeance 16GB", Category = "RAM", Brand = "Corsair", Description = "16GB, 3200MHz, RGB", ImagePath = "", Price = 3500, Quantity = 12 },
        new Product { SKU = "RM-GS003", ProductName = "G.Skill Ripjaws 32GB", Category = "RAM", Brand = "G.Skill", Description = "32GB, 3600MHz, CL18", ImagePath = "", Price = 6000, Quantity = 8 },
        new Product { SKU = "RM-HX004", ProductName = "HyperX Predator 8GB", Category = "RAM", Brand = "HyperX", Description = "8GB, 3200MHz, CL16", ImagePath = "", Price = 2000, Quantity = 15 },
        new Product { SKU = "RM-CR005", ProductName = "Crucial Ballistix 16GB", Category = "RAM", Brand = "Crucial", Description = "16GB, 3200MHz, RGB", ImagePath = "", Price = 3200, Quantity = 10 },
        new Product { SKU = "RM-TM006", ProductName = "TeamGroup T-Force 8GB", Category = "RAM", Brand = "TeamGroup", Description = "8GB, 2666MHz, CL19", ImagePath = "", Price = 1500, Quantity = 18 },
        new Product { SKU = "RM-AD007", ProductName = "ADATA XPG 16GB", Category = "RAM", Brand = "ADATA", Description = "16GB, 3200MHz, RGB", ImagePath = "", Price = 3000, Quantity = 9 },
        new Product { SKU = "RM-PT008", ProductName = "Patriot Viper 32GB", Category = "RAM", Brand = "Patriot", Description = "32GB, 3600MHz, RGB", ImagePath = "", Price = 5500, Quantity = 6 },
        new Product { SKU = "RM-SS009", ProductName = "Samsung DDR4 8GB", Category = "RAM", Brand = "Samsung", Description = "8GB, 2666MHz, OEM", ImagePath = "", Price = 1600, Quantity = 12 },
        new Product { SKU = "RM-SP010", ProductName = "Silicon Power 16GB", Category = "RAM", Brand = "Silicon Power", Description = "16GB, 3200MHz", ImagePath = "", Price = 2800, Quantity = 8 },

        // GPUs
        new Product { SKU = "GC-NV001", ProductName = "NVIDIA GTX 1650", Category = "GPU", Brand = "NVIDIA", Description = "4GB GDDR6, Dual-Fan", ImagePath = "", Price = 10000, Quantity = 8 },
        new Product { SKU = "GC-NV002", ProductName = "RTX 3050", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, DLSS Support", ImagePath = "", Price = 15000, Quantity = 6 },
        new Product { SKU = "GC-NV003", ProductName = "RTX 3060 Ti", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, Ray Tracing", ImagePath = "", Price = 25000, Quantity = 5 },
        new Product { SKU = "GC-NV004", ProductName = "RTX 3070", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, High-End Gaming", ImagePath = "", Price = 35000, Quantity = 4 },
        new Product { SKU = "GC-NV005", ProductName = "RTX 3080", Category = "GPU", Brand = "NVIDIA", Description = "10GB GDDR6X, 4K Gaming", ImagePath = "", Price = 50000, Quantity = 3 },
        new Product { SKU = "GC-AM006", ProductName = "AMD RX 6600", Category = "GPU", Brand = "AMD", Description = "8GB GDDR6, 1080p Gaming", ImagePath = "", Price = 18000, Quantity = 7 },
        new Product { SKU = "GC-AM007", ProductName = "RX 6700 XT", Category = "GPU", Brand = "AMD", Description = "12GB GDDR6, 1440p Gaming", ImagePath = "", Price = 30000, Quantity = 5 },
        new Product { SKU = "GC-AM008", ProductName = "RX 6800", Category = "GPU", Brand = "AMD", Description = "16GB GDDR6, 4K Gaming", ImagePath = "", Price = 40000, Quantity = 3 },
        new Product { SKU = "GC-NV009", ProductName = "GT 1030 (Budget)", Category = "GPU", Brand = "NVIDIA", Description = "2GB GDDR5, Basic Use", ImagePath = "", Price = 5000, Quantity = 10 },
        new Product { SKU = "GC-IN010", ProductName = "Intel Arc A750", Category = "GPU", Brand = "Intel", Description = "8GB GDDR6, Mid-Range", ImagePath = "", Price = 12000, Quantity = 6 },

        // Motherboards
        new Product { SKU = "MB-AS001", ProductName = "ASUS ROG Strix B550-F", Category = "Motherboards", Brand = "ASUS", Description = "AMD AM4, ATX, RGB", ImagePath = "", Price = 8000, Quantity = 7 },
        new Product { SKU = "MB-MS002", ProductName = "MSI B450 Tomahawk", Category = "Motherboards", Brand = "MSI", Description = "AMD AM4, ATX, Military Grade", ImagePath = "", Price = 5500, Quantity = 9 },
        new Product { SKU = "MB-GB003", ProductName = "Gigabyte B760M", Category = "Motherboards", Brand = "Gigabyte", Description = "Intel LGA1700, mATX, DDR4", ImagePath = "", Price = 7000, Quantity = 8 },
        new Product { SKU = "MB-AR004", ProductName = "ASRock H610M", Category = "Motherboards", Brand = "ASRock", Description = "Intel LGA1700, mATX, Budget", ImagePath = "", Price = 4500, Quantity = 12 },
        new Product { SKU = "MB-AS005", ProductName = "ASUS TUF Gaming X570", Category = "Motherboards", Brand = "ASUS", Description = "AMD AM4, ATX, PCIe 4.0", ImagePath = "", Price = 10000, Quantity = 5 },
        new Product { SKU = "MB-MS006", ProductName = "MSI MAG Z790", Category = "Motherboards", Brand = "MSI", Description = "Intel LGA1700, ATX, DDR5", ImagePath = "", Price = 12000, Quantity = 4 },
        new Product { SKU = "MB-GB007", ProductName = "Gigabyte A520M", Category = "Motherboards", Brand = "Gigabyte", Description = "AMD AM4, mATX, Budget", ImagePath = "", Price = 3500, Quantity = 10 },
        new Product { SKU = "MB-AS008", ProductName = "ASUS Prime H510M", Category = "Motherboards", Brand = "ASUS", Description = "Intel LGA1200, mATX, Basic", ImagePath = "", Price = 4000, Quantity = 8 },
        new Product { SKU = "MB-BT009", ProductName = "Biostar B550MH", Category = "Motherboards", Brand = "Biostar", Description = "AMD AM4, mATX, Budget", ImagePath = "", Price = 3000, Quantity = 6 },
        new Product { SKU = "MB-AR010", ProductName = "ASRock Z690", Category = "Motherboards", Brand = "ASRock", Description = "Intel LGA1700, ATX, High-End", ImagePath = "", Price = 11000, Quantity = 5 },

        // Accessories
        new Product { SKU = "AC-LG001", ProductName = "Logitech Webcam (C920)", Category = "Accessories", Brand = "Logitech", Description = "1080p, Built-in Mic", ImagePath = "", Price = 3500, Quantity = 10 },
        new Product { SKU = "AC-BL002", ProductName = "Blue Snowball Mic", Category = "Accessories", Brand = "Blue", Description = "USB Condenser Mic", ImagePath = "", Price = 4000, Quantity = 8 },
        new Product { SKU = "AC-WA003", ProductName = "Wacom Drawing Tablet", Category = "Accessories", Brand = "Wacom", Description = "Small, Pressure-Sensitive", ImagePath = "", Price = 5000, Quantity = 6 },
        new Product { SKU = "AC-TP004", ProductName = "TP-Link WiFi Adapter", Category = "Accessories", Brand = "TP-Link", Description = "USB, Dual-Band", ImagePath = "", Price = 1200, Quantity = 15 },
        new Product { SKU = "AC-AN005", ProductName = "Anker USB Hub", Category = "Accessories", Brand = "Anker", Description = "4-Port USB 3.0", ImagePath = "", Price = 1500, Quantity = 12 },
        new Product { SKU = "AC-UG006", ProductName = "UGREEN HDMI Cable", Category = "Accessories", Brand = "UGREEN", Description = "2m, 4K Support", ImagePath = "", Price = 800, Quantity = 20 },
        new Product { SKU = "AC-JB007", ProductName = "JBL Pebble Speakers", Category = "Accessories", Brand = "JBL", Description = "USB-Powered, Compact", ImagePath = "", Price = 1800, Quantity = 10 },
        new Product { SKU = "AC-XB008", ProductName = "Xbox Wireless Controller", Category = "Accessories", Brand = "Microsoft", Description = "Wireless, Bluetooth", ImagePath = "", Price = 3000, Quantity = 9 },
        new Product { SKU = "AC-NZ009", ProductName = "NZXT RGB LED Strip", Category = "Accessories", Brand = "NZXT", Description = "Magnetic, RGB Lighting", ImagePath = "", Price = 2000, Quantity = 7 },
        new Product { SKU = "AC-CM010", ProductName = "Cooler Master Mousepad", Category = "Accessories", Brand = "Cooler Master", Description = "Large, Non-Slip", ImagePath = "", Price = 1000, Quantity = 15 }
    };

            foreach (var product in products)
            {
                AddProduct(product);
            }

            Debug.WriteLine("Seeded products into database.");
        }
        public void SeedProductsWithImage()
        {
            var products = new List<Product>
    {
        // CPUs
        new Product { SKU = "CP-I510", ProductName = "Intel i510400", Category = "CPU", Brand = "Intel", Description = "i3 I510400", ImagePath = "Assets/Images/noImage.jpeg", Price = 10000, Quantity = 5 },
        
        // Laptops
        new Product { SKU = "LP-AS001", ProductName = "ASUS TUF Gaming", Category = "Laptops", Brand = "ASUS", Description = "15.6\" FHD, Ryzen 5, GTX 1650, 8GB RAM", ImagePath = "Assets/Images/LP-AS001.png", Price = 45000, Quantity = 10 },
        new Product { SKU = "LP-AC002", ProductName = "Acer Nitro 5", Category = "Laptops", Brand = "Acer", Description = "15.6\" FHD, i5, RTX 3050, 16GB RAM", ImagePath = "Assets/Images/LP-AC002.webp", Price = 50000, Quantity = 8 },
        new Product { SKU = "LP-LN003", ProductName = "Lenovo IdeaPad", Category = "Laptops", Brand = "Lenovo", Description = "14\" FHD, Ryzen 3, 8GB RAM, 256GB SSD", ImagePath = "Assets/Images/LP-LN003.jfif", Price = 25000, Quantity = 12 },
        new Product { SKU = "LP-HP004", ProductName = "HP Pavilion", Category = "Laptops", Brand = "HP", Description = "15.6\" FHD, i5, 8GB RAM, 512GB SSD", ImagePath = "Assets/Images/LP-HP004.jfif", Price = 35000, Quantity = 7 },
        new Product { SKU = "LP-DL005", ProductName = "Dell Inspiron", Category = "Laptops", Brand = "Dell", Description = "15.6\" FHD, i3, 8GB RAM, 1TB HDD", ImagePath = "Assets/Images/LP-DL005.jfif", Price = 28000, Quantity = 9 },
        new Product { SKU = "LP-MS006", ProductName = "MSI GF63 Thin", Category = "Laptops", Brand = "MSI", Description = "15.6\" FHD, i7, GTX 1650, 16GB RAM", ImagePath = "Assets/Images/LP-MS006.jfif", Price = 55000, Quantity = 5 },
        new Product { SKU = "LP-AP007", ProductName = "Apple MacBook Air", Category = "Laptops", Brand = "Apple", Description = "13.3\" Retina, M1, 8GB RAM, 256GB SSD", ImagePath = "Assets/Images/LP-AP007.jfif", Price = 60000, Quantity = 5 },
        new Product { SKU = "LP-HW008", ProductName = "Huawei MateBook", Category = "Laptops", Brand = "Huawei", Description = "14\" FHD, i5, 8GB RAM, 512GB SSD", ImagePath = "Assets/Images/LP-HW008.jfif", Price = 40000, Quantity = 4 },
        new Product { SKU = "LP-GB009", ProductName = "Gigabyte G5", Category = "Laptops", Brand = "Gigabyte", Description = "15.6\" FHD, i5, RTX 3050, 16GB RAM", ImagePath = "Assets/Images/LP-GB009.jfif", Price = 52000, Quantity = 3 },
        new Product { SKU = "LP-RZ010", ProductName = "Razer Blade", Category = "Laptops", Brand = "Razer", Description = "15.6\" QHD, i7, RTX 3060, 16GB RAM", ImagePath = "Assets/Images/LP-RZ010.jfif", Price = 90000, Quantity = 2 },

        // Monitors
        new Product { SKU = "MN-AS001", ProductName = "ASUS TUF Gaming (144Hz)", Category = "Monitors", Brand = "ASUS", Description = "24\" FHD, 144Hz, 1ms", ImagePath = "Assets/Images/MN-AS001.jfif", Price = 12000, Quantity = 8 },
        new Product { SKU = "MN-AC002", ProductName = "Acer Nitro (IPS Panel)", Category = "Monitors", Brand = "Acer", Description = "23.8\" FHD, IPS, 75Hz", ImagePath = "Assets/Images/MN-AC002.jfif", Price = 8500, Quantity = 10 },
        new Product { SKU = "MN-SS003", ProductName = "Samsung Odyssey (Curved)", Category = "Monitors", Brand = "Samsung", Description = "27\" QHD, 144Hz, Curved", ImagePath = "Assets/Images/MN-SS003.jfif", Price = 18000, Quantity = 5 },
        new Product { SKU = "MN-LG004", ProductName = "LG UltraGear (4K)", Category = "Monitors", Brand = "LG", Description = "27\" 4K, HDR, 144Hz", ImagePath = "Assets/Images/MN-LG004.jfif", Price = 25000, Quantity = 4 },
        new Product { SKU = "MN-DL005", ProductName = "Dell Ultrasharp (Office)", Category = "Monitors", Brand = "Dell", Description = "24\" FHD, IPS, 60Hz", ImagePath = "Assets/Images/MN-DL005.jfif", Price = 10000, Quantity = 7 },
        new Product { SKU = "MN-MS006", ProductName = "MSI Optix (Gaming)", Category = "Monitors", Brand = "MSI", Description = "27\" FHD, 165Hz, 1ms", ImagePath = "Assets/Images/MN-MS006.jfif", Price = 15000, Quantity = 6 },
        new Product { SKU = "MN-VS007", ProductName = "ViewSonic (Budget)", Category = "Monitors", Brand = "ViewSonic", Description = "21.5\" FHD, 60Hz", ImagePath = "Assets/Images/MN-VS007.jfif", Price = 6500, Quantity = 12 },
        new Product { SKU = "MN-BQ008", ProductName = "BenQ (Designer)", Category = "Monitors", Brand = "BenQ", Description = "24\" FHD, IPS, 99% sRGB", ImagePath = "Assets/Images/MN-BQ008.jfif", Price = 14000, Quantity = 5 },
        new Product { SKU = "MN-PH009", ProductName = "Philips (EyeCare)", Category = "Monitors", Brand = "Philips", Description = "22\" FHD, Low Blue Light", ImagePath = "Assets/Images/MN-PH009.jfif", Price = 7500, Quantity = 9 },
        new Product { SKU = "MN-HP010", ProductName = "HP Pavilion (Basic)", Category = "Monitors", Brand = "HP", Description = "21.5\" FHD, 60Hz", ImagePath = "Assets/Images/MN-HP010.jfif", Price = 7000, Quantity = 8 },

        // Keyboards
        new Product { SKU = "KB-RD001", ProductName = "Redragon K552 (Mechanical)", Category = "Keyboards", Brand = "Redragon", Description = "Mechanical, RGB, Blue Switches", ImagePath = "Assets/Images/KB-RD001.jfif", Price = 2500, Quantity = 15 },
        new Product { SKU = "KB-RZ002", ProductName = "Razer BlackWidow (RGB)", Category = "Keyboards", Brand = "Razer", Description = "Mechanical, Chroma RGB", ImagePath = "Assets/Images/KB-RZ002.jfif", Price = 6000, Quantity = 8 },
        new Product { SKU = "KB-LG003", ProductName = "Logitech K120 (Membrane)", Category = "Keyboards", Brand = "Logitech", Description = "Membrane, Spill-Resistant", ImagePath = "Assets/Images/KB-LG003.jfif", Price = 800, Quantity = 20 },
        new Product { SKU = "KB-RK004", ProductName = "Royal Kludge RK61 (Wireless)", Category = "Keyboards", Brand = "Royal Kludge", Description = "60% Wireless, Mechanical", ImagePath = "Assets/Images/KB-RK004.jfif", Price = 3500, Quantity = 10 },
        new Product { SKU = "KB-CR005", ProductName = "Corsair K70 (Gaming)", Category = "Keyboards", Brand = "Corsair", Description = "Mechanical, RGB, Aluminum Frame", ImagePath = "Assets/Images/KB-CR005.jfif", Price = 7000, Quantity = 6 },
        new Product { SKU = "KB-HX006", ProductName = "HyperX Alloy FPS", Category = "Keyboards", Brand = "HyperX", Description = "Mechanical, Red Switches", ImagePath = "Assets/Images/KB-HX006.jfif", Price = 4500, Quantity = 9 },
        new Product { SKU = "KB-ST007", ProductName = "SteelSeries Apex", Category = "Keyboards", Brand = "SteelSeries", Description = "Hybrid Mechanical, RGB", ImagePath = "Assets/Images/KB-ST007.jfif", Price = 5500, Quantity = 7 },
        new Product { SKU = "KB-RC008", ProductName = "Roccat Vulcan", Category = "Keyboards", Brand = "Roccat", Description = "Mechanical, RGB, Low-Profile", ImagePath = "Assets/Images/KB-RC008.jfif", Price = 6500, Quantity = 4 },
        new Product { SKU = "KB-CM009", ProductName = "Cooler Master CK550", Category = "Keyboards", Brand = "Cooler Master", Description = "Mechanical, RGB, Gateron Switches", ImagePath = "Assets/Images/KB-CM009.jfif", Price = 4000, Quantity = 8 },
        new Product { SKU = "KB-AP010", ProductName = "Apple Magic Keyboard", Category = "Keyboards", Brand = "Apple", Description = "Wireless, Slim Design", ImagePath = "Assets/Images/KB-AP010.jfif", Price = 5000, Quantity = 5 },

        // Mice
        new Product { SKU = "MS-LG001", ProductName = "Logitech G102 (Budget Gaming)", Category = "Mice", Brand = "Logitech", Description = "Optical, RGB, 8K DPI", ImagePath = "Assets/Images/MS-LG001.jfif", Price = 1200, Quantity = 20 },
        new Product { SKU = "MS-RZ002", ProductName = "Razer DeathAdder (Ergonomic)", Category = "Mice", Brand = "Razer", Description = "Ergonomic, 16K DPI", ImagePath = "Assets/Images/MS-RZ002.jfif", Price = 3000, Quantity = 12 },
        new Product { SKU = "MS-ST003", ProductName = "SteelSeries Rival 3", Category = "Mice", Brand = "SteelSeries", Description = "Lightweight, 8.5K DPI", ImagePath = "Assets/Images/MS-ST003.jfif", Price = 2000, Quantity = 15 },
        new Product { SKU = "MS-RD004", ProductName = "Redragon M711 (RGB)", Category = "Mice", Brand = "Redragon", Description = "RGB, 10K DPI, 7 Buttons", ImagePath = "Assets/Images/MS-RD004.jfif", Price = 1500, Quantity = 18 },
        new Product { SKU = "MS-CR005", ProductName = "Corsair Harpoon", Category = "Mice", Brand = "Corsair", Description = "Wireless, 10K DPI", ImagePath = "Assets/Images/MS-CR005.jfif", Price = 2500, Quantity = 10 },
        new Product { SKU = "MS-AS006", ProductName = "ASUS ROG Gladius", Category = "Mice", Brand = "ASUS", Description = "RGB, 12K DPI, Swappable Switches", ImagePath = "Assets/Images/MS-AS006.jfif", Price = 3500, Quantity = 8 },
        new Product { SKU = "MS-HX007", ProductName = "HyperX Pulsefire", Category = "Mice", Brand = "HyperX", Description = "RGB, 16K DPI, Lightweight", ImagePath = "Assets/Images/MS-HX007.jfif", Price = 2800, Quantity = 9 },
        new Product { SKU = "MS-MS008", ProductName = "Microsoft Classic", Category = "Mice", Brand = "Microsoft", Description = "Wired, Basic Office Mouse", ImagePath = "Assets/Images/MS-MS008.jfif", Price = 600, Quantity = 25 },
        new Product { SKU = "MS-HP009", ProductName = "HP X1000 (Basic)", Category = "Mice", Brand = "HP", Description = "Wired, 1.6K DPI", ImagePath = "Assets/Images/MS-HP009.jfif", Price = 500, Quantity = 30 },
        new Product { SKU = "MS-AP010", ProductName = "Apple Magic Mouse", Category = "Mice", Brand = "Apple", Description = "Wireless, Multi-Touch", ImagePath = "Assets/Images/MS-AP010.jfif", Price = 4000, Quantity = 6 },

        // Storage
        new Product { SKU = "ST-SG001", ProductName = "Seagate Barracuda 1TB HDD", Category = "Storage", Brand = "Seagate", Description = "3.5\", 7200 RPM", ImagePath = "Assets/Images/ST-SG001.jfif", Price = 2500, Quantity = 15 },
        new Product { SKU = "ST-WD002", ProductName = "WD Blue 2TB HDD", Category = "Storage", Brand = "WD", Description = "3.5\", 5400 RPM", ImagePath = "Assets/Images/ST-WD002.jfif", Price = 3500, Quantity = 12 },
        new Product { SKU = "ST-SS003", ProductName = "Samsung 870 EVO (SATA SSD)", Category = "Storage", Brand = "Samsung", Description = "500GB, SATA III", ImagePath = "Assets/Images/ST-SS003.jfif", Price = 3000, Quantity = 10 },
        new Product { SKU = "ST-KN004", ProductName = "Kingston A400 (120GB SSD)", Category = "Storage", Brand = "Kingston", Description = "120GB, SATA III", ImagePath = "Assets/Images/ST-KN004.jfif", Price = 1200, Quantity = 20 },
        new Product { SKU = "ST-CR005", ProductName = "Crucial MX500 (500GB SSD)", Category = "Storage", Brand = "Crucial", Description = "500GB, SATA III", ImagePath = "Assets/Images/ST-CR005.jfif", Price = 2800, Quantity = 8 },
        new Product { SKU = "ST-WD006", ProductName = "WD Black SN750 (NVMe)", Category = "Storage", Brand = "WD", Description = "500GB, NVMe PCIe", ImagePath = "Assets/Images/ST-WD006.jfif", Price = 4000, Quantity = 6 },
        new Product { SKU = "ST-SD007", ProductName = "SanDisk Ultra 3D SSD", Category = "Storage", Brand = "SanDisk", Description = "1TB, SATA III", ImagePath = "Assets/Images/ST-SD007.jfif", Price = 5000, Quantity = 5 },
        new Product { SKU = "ST-TS008", ProductName = "Toshiba P300 HDD", Category = "Storage", Brand = "Toshiba", Description = "1TB, 7200 RPM", ImagePath = "Assets/Images/ST-TS008.jfif", Price = 2300, Quantity = 10 },
        new Product { SKU = "ST-AD009", ProductName = "ADATA SU800 SSD", Category = "Storage", Brand = "ADATA", Description = "256GB, SATA III", ImagePath = "Assets/Images/ST-AD009.jfif", Price = 1800, Quantity = 12 },
        new Product { SKU = "ST-SS010", ProductName = "Samsung 980 Pro (PCIe 4.0)", Category = "Storage", Brand = "Samsung", Description = "1TB, NVMe PCIe 4.0", ImagePath = "Assets/Images/ST-SS010.jfif", Price = 7000, Quantity = 4 },

        // RAM
        new Product { SKU = "RM-KN001", ProductName = "Kingston Fury 8GB DDR4", Category = "RAM", Brand = "Kingston", Description = "8GB, 3200MHz, CL16", ImagePath = "Assets/Images/RM-KN001.jpg", Price = 1800, Quantity = 20 },
        new Product { SKU = "RM-CR002", ProductName = "Corsair Vengeance 16GB", Category = "RAM", Brand = "Corsair", Description = "16GB, 3200MHz, RGB", ImagePath = "Assets/Images/RM-CR002.jfif", Price = 3500, Quantity = 12 },
        new Product { SKU = "RM-GS003", ProductName = "G.Skill Ripjaws 32GB", Category = "RAM", Brand = "G.Skill", Description = "32GB, 3600MHz, CL18", ImagePath = "Assets/Images/RM-GS003.jfif", Price = 6000, Quantity = 8 },
        new Product { SKU = "RM-HX004", ProductName = "HyperX Predator 8GB", Category = "RAM", Brand = "HyperX", Description = "8GB, 3200MHz, CL16", ImagePath = "Assets/Images/RM-HX004.jfif", Price = 2000, Quantity = 15 },
        new Product { SKU = "RM-CR005", ProductName = "Crucial Ballistix 16GB", Category = "RAM", Brand = "Crucial", Description = "16GB, 3200MHz, RGB", ImagePath = "Assets/Images/RM-CR005.jfif", Price = 3200, Quantity = 10 },
        new Product { SKU = "RM-TM006", ProductName = "TeamGroup T-Force 8GB", Category = "RAM", Brand = "TeamGroup", Description = "8GB, 2666MHz, CL19", ImagePath = "Assets/Images/RM-TM006.jfif", Price = 1500, Quantity = 18 },
        new Product { SKU = "RM-AD007", ProductName = "ADATA XPG 16GB", Category = "RAM", Brand = "ADATA", Description = "16GB, 3200MHz, RGB", ImagePath = "Assets/Images/RM-AD007.jfif", Price = 3000, Quantity = 9 },
        new Product { SKU = "RM-PT008", ProductName = "Patriot Viper 32GB", Category = "RAM", Brand = "Patriot", Description = "32GB, 3600MHz, RGB", ImagePath = "Assets/Images/RM-PT008.jfif", Price = 5500, Quantity = 6 },
        new Product { SKU = "RM-SS009", ProductName = "Samsung DDR4 8GB", Category = "RAM", Brand = "Samsung", Description = "8GB, 2666MHz, OEM", ImagePath = "Assets/Images/RM-SS009.jfif", Price = 1600, Quantity = 12 },
        new Product { SKU = "RM-SP010", ProductName = "Silicon Power 16GB", Category = "RAM", Brand = "Silicon Power", Description = "16GB, 3200MHz", ImagePath = "Assets/Images/RM-SP010.jfif", Price = 2800, Quantity = 8 },

        // GPUs
        new Product { SKU = "GC-NV001", ProductName = "NVIDIA GTX 1650", Category = "GPU", Brand = "NVIDIA", Description = "4GB GDDR6, Dual-Fan", ImagePath = "Assets/Images/GC-NV001.jfif", Price = 10000, Quantity = 8 },
        new Product { SKU = "GC-NV002", ProductName = "RTX 3050", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, DLSS Support", ImagePath = "Assets/Images/GC-NV002.jfif", Price = 15000, Quantity = 6 },
        new Product { SKU = "GC-NV003", ProductName = "RTX 3060 Ti", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, Ray Tracing", ImagePath = "Assets/Images/GC-NV003.jfif", Price = 25000, Quantity = 5 },
        new Product { SKU = "GC-NV004", ProductName = "RTX 3070", Category = "GPU", Brand = "NVIDIA", Description = "8GB GDDR6, High-End Gaming", ImagePath = "Assets/Images/GC-NV004.jfif", Price = 35000, Quantity = 4 },
        new Product { SKU = "GC-NV005", ProductName = "RTX 3080", Category = "GPU", Brand = "NVIDIA", Description = "10GB GDDR6X, 4K Gaming", ImagePath = "Assets/Images/GC-NV005.jfif", Price = 50000, Quantity = 3 },
        new Product { SKU = "GC-AM006", ProductName = "AMD RX 6600", Category = "GPU", Brand = "AMD", Description = "8GB GDDR6, 1080p Gaming", ImagePath = "Assets/Images/GC-AM006.jfif", Price = 18000, Quantity = 6 },
        new Product { SKU = "GC-AM007", ProductName = "RX 6700 XT", Category = "GPU", Brand = "AMD", Description = "12GB GDDR6, 1440p Gaming", ImagePath = "Assets/Images/GC-AM007.jfif", Price = 30000, Quantity = 5 },
        new Product { SKU = "GC-AM008", ProductName = "RX 6800", Category = "GPU", Brand = "AMD", Description = "16GB GDDR6, 4K Gaming", ImagePath = "Assets/Images/GC-AM008.jfif", Price = 40000, Quantity = 3 },
        new Product { SKU = "GC-NV009", ProductName = "GT 1030 (Budget)", Category = "GPU", Brand = "NVIDIA", Description = "2GB GDDR5, Basic Use", ImagePath = "Assets/Images/GC-NV009.jfif", Price = 5000, Quantity = 10 },
        new Product { SKU = "GC-IN010", ProductName = "Intel Arc A750", Category = "GPU", Brand = "Intel", Description = "8GB GDDR6, Mid-Range", ImagePath = "Assets/Images/GC-IN010.jfif", Price = 12000, Quantity = 6 },

        // Motherboards
        new Product { SKU = "MB-AS001", ProductName = "ASUS ROG Strix B550-F", Category = "Motherboards", Brand = "ASUS", Description = "AMD AM4, ATX, RGB", ImagePath = "Assets/Images/MB-AS001.jfif", Price = 8000, Quantity = 7 },
        new Product { SKU = "MB-MS002", ProductName = "MSI B450 Tomahawk", Category = "Motherboards", Brand = "MSI", Description = "AMD AM4, ATX, Military Grade", ImagePath = "Assets/Images/MB-MS002.jfif", Price = 5500, Quantity = 9 },
        new Product { SKU = "MB-GB003", ProductName = "Gigabyte B760M", Category = "Motherboards", Brand = "Gigabyte", Description = "Intel LGA1700, mATX, DDR4", ImagePath = "Assets/Images/MB-GB003.jfif", Price = 7000, Quantity = 8 },
        new Product { SKU = "MB-AR004", ProductName = "ASRock H610M", Category = "Motherboards", Brand = "ASRock", Description = "Intel LGA1700, mATX, Budget", ImagePath = "Assets/Images/MB-AR004.jfif", Price = 4500, Quantity = 12 },
        new Product { SKU = "MB-AS005", ProductName = "ASUS TUF Gaming X570", Category = "Motherboards", Brand = "ASUS", Description = "AMD AM4, ATX, PCIe 4.0", ImagePath = "Assets/Images/MB-AS005.jfif", Price = 10000, Quantity = 5 },
        new Product { SKU = "MB-MS006", ProductName = "MSI MAG Z790", Category = "Motherboards", Brand = "MSI", Description = "Intel LGA1700, ATX, DDR5", ImagePath = "Assets/Images/MB-MS006.jfif", Price = 12000, Quantity = 4 },
        new Product { SKU = "MB-GB007", ProductName = "Gigabyte A520M", Category = "Motherboards", Brand = "Gigabyte", Description = "AMD AM4, mATX, Budget", ImagePath = "Assets/Images/MB-GB007.jfif", Price = 3500, Quantity = 10 },
        new Product { SKU = "MB-AS008", ProductName = "ASUS Prime H510M", Category = "Motherboards", Brand = "ASUS", Description = "Intel LGA1200, mATX, Basic", ImagePath = "Assets/Images/MB-AS008.jfif", Price = 4000, Quantity = 8 },
        new Product { SKU = "MB-BT009", ProductName = "Biostar B550MH", Category = "Motherboards", Brand = "Biostar", Description = "AMD AM4, mATX, Budget", ImagePath = "Assets/Images/MB-BT009.jfif", Price = 3000, Quantity = 6 },
        new Product { SKU = "MB-AR010", ProductName = "ASRock Z690", Category = "Motherboards", Brand = "ASRock", Description = "Intel LGA1700, ATX, High-End", ImagePath = "Assets/Images/MB-AR010.jfif", Price = 11000, Quantity = 5 },

        // Accessories
        new Product { SKU = "AC-LG001", ProductName = "Logitech Webcam (C920)", Category = "Accessories", Brand = "Logitech", Description = "1080p, Built-in Mic", ImagePath = "Assets/Images/AC-LG001.jfif", Price = 3500, Quantity = 10 },
        new Product { SKU = "AC-BL002", ProductName = "Blue Snowball Mic", Category = "Accessories", Brand = "Blue", Description = "USB Condenser Mic", ImagePath = "Assets/Images/AC-BL002.jfif", Price = 4000, Quantity = 8 },
        new Product { SKU = "AC-WA003", ProductName = "Wacom Drawing Tablet", Category = "Accessories", Brand = "Wacom", Description = "Small, Pressure-Sensitive", ImagePath = "Assets/Images/AC-WA003.jfif", Price = 5000, Quantity = 6 },
        new Product { SKU = "AC-TP004", ProductName = "TP-Link WiFi Adapter", Category = "Accessories", Brand = "TP-Link", Description = "USB, Dual-Band", ImagePath = "Assets/Images/AC-TP004.jfif", Price = 1200, Quantity = 15 },
        new Product { SKU = "AC-AN005", ProductName = "Anker USB Hub", Category = "Accessories", Brand = "Anker", Description = "4-Port USB 3.0", ImagePath = "Assets/Images/AC-AN005.jfif", Price = 1500, Quantity = 12 },
        new Product { SKU = "AC-UG006", ProductName = "UGREEN HDMI Cable", Category = "Accessories", Brand = "UGREEN", Description = "2m, 4K Support", ImagePath = "Assets/Images/AC-UG006.jfif", Price = 800, Quantity = 20 },
        new Product { SKU = "AC-JB007", ProductName = "JBL Pebble Speakers", Category = "Accessories", Brand = "JBL", Description = "USB-Powered, Compact", ImagePath = "Assets/Images/AC-JB007.jfif", Price = 1800, Quantity = 10 },
        new Product { SKU = "AC-XB008", ProductName = "Xbox Wireless Controller", Category = "Accessories", Brand = "Microsoft", Description = "Wireless, Bluetooth", ImagePath = "Assets/Images/AC-XB008.jfif", Price = 3000, Quantity = 9 },
        new Product { SKU = "AC-NZ009", ProductName = "NZXT RGB LED Strip", Category = "Accessories", Brand = "NZXT", Description = "Magnetic, RGB Lighting", ImagePath = "Assets/Images/AC-NZ009.jfif", Price = 2000, Quantity = 7 },
        new Product { SKU = "AC-CM010", ProductName = "Cooler Master Mousepad", Category = "Accessories", Brand = "Cooler Master", Description = "Large, Non-Slip", ImagePath = "Assets/Images/AC-CM010.jfif", Price = 1000, Quantity = 15 },

        // Desktops & PCs
        new Product { SKU = "DT-PG001", ProductName = "Pre-built Gaming PC", Category = "Desktops & PCs", Brand = "Custom", Description = "Ryzen 5, GTX 1660, 16GB RAM, 512GB SSD", ImagePath = "Assets/Images/DT-PG001.jfif", Price = 35000, Quantity = 5 },
        new Product { SKU = "DT-OD002", ProductName = "Office Desktop", Category = "Desktops & PCs", Brand = "HP", Description = "Intel i3, 8GB RAM, 1TB HDD", ImagePath = "Assets/Images/DT-OD002.jfif", Price = 18000, Quantity = 10 },
        new Product { SKU = "DT-AI003", ProductName = "All-in-One PC", Category = "Desktops & PCs", Brand = "Acer", Description = "21.5\" FHD, i5, 8GB RAM, 256GB SSD", ImagePath = "Assets/Images/DT-AI003.jfif", Price = 30000, Quantity = 6 },
        new Product { SKU = "DT-MN004", ProductName = "Mini PC (Intel NUC)", Category = "Desktops & PCs", Brand = "Intel", Description = "Compact, i5, 8GB RAM, 256GB SSD", ImagePath = "Assets/Images/DT-MN004.jfif", Price = 25000, Quantity = 4 },
        new Product { SKU = "DT-WS005", ProductName = "Workstation PC", Category = "Desktops & PCs", Brand = "Dell", Description = "Intel Xeon, 32GB RAM, 1TB SSD", ImagePath = "Assets/Images/DT-WS005.jfif", Price = 80000, Quantity = 3 },
        new Product { SKU = "DT-CG006", ProductName = "Custom Gaming PC", Category = "Desktops & PCs", Brand = "Custom", Description = "i7, RTX 3060, 16GB RAM, 1TB SSD", ImagePath = "Assets/Images/DT-CG006.jfif", Price = 65000, Quantity = 7 },
        new Product { SKU = "DT-BP007", ProductName = "Budget PC", Category = "Desktops & PCs", Brand = "Custom", Description = "Pentium Gold, 4GB RAM, 500GB HDD", ImagePath = "Assets/Images/DT-BP007.jfif", Price = 12000, Quantity = 8 },
        new Product { SKU = "DT-HT008", ProductName = "Home Theater PC (HTPC)", Category = "Desktops & PCs", Brand = "Custom", Description = "Compact, Ryzen 3, 8GB RAM, 256GB SSD", ImagePath = "Assets/Images/DT-HT008.jfif", Price = 20000, Quantity = 2 },
        new Product { SKU = "DT-SF009", ProductName = "SFF (Small Form Factor) PC", Category = "Desktops & PCs", Brand = "ASUS", Description = "Compact, i5, 8GB RAM, 512GB SSD", ImagePath = "Assets/Images/DT-SF009.jfif", Price = 28000, Quantity = 3 },
        new Product { SKU = "DT-HE010", ProductName = "High-End Editing PC", Category = "Desktops & PCs", Brand = "Custom", Description = "i9, RTX 4080, 32GB RAM, 2TB SSD", ImagePath = "Assets/Images/DT-HE010.jfif", Price = 120000, Quantity = 2 }
    };

            foreach (var product in products)
            {
                AddProduct(product);
            }

            Debug.WriteLine("Seeded products into database.");
        }
        public void RemoveProduct(int productId)
        {
            Connect();
            try
            {
                string query = "DELETE FROM Products WHERE Id = @Id";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@Id", productId);
                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }

        public void UpdateProduct(int id, string sku, string name, string category, string brand, string? description, string? imagePath, decimal price, int quantity)
        {
            Connect();
            try
            {
                string query = @"
            UPDATE Products SET 
                SKU = @SKU,
                ProductName = @ProductName,
                Category = @Category,
                Brand = @Brand,
                Description = @Description,
                ImagePath = @ImagePath,
                Price = @Price,
                Quantity = @Quantity
            WHERE Id = @Id";

                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@SKU", sku);
                command.Parameters.AddWithValue("@ProductName", name);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Brand", brand);
                command.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImagePath", imagePath ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
                int affectedRows = command.ExecuteNonQuery();
                Debug.WriteLine($"Rows affected: {affectedRows}");
            }
            finally
            {
                Disconnect();
            }
        }

        public async Task UpdateProductStockAsync(string sku, int quantityDelta)
        {
            Connect();
            try
            {
                // The query increases or decreases stock by quantityDelta
                string query = "UPDATE Products SET Quantity = Quantity + @QuantityDelta WHERE SKU = @SKU";

                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@QuantityDelta", quantityDelta);
                command.Parameters.AddWithValue("@SKU", sku);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                Disconnect();
            }
        }

        public List<InventoryReport> GetInventoryReport()
        {
            Connect();
            var list = new List<InventoryReport>();
            try
            {
                string query = "SELECT ProductName, Category, Quantity, Price FROM Products";
                using var cmd = new SqliteCommand(query, _connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var report = new InventoryReport
                    {
                        ProductName = reader.GetString(0),
                        Category = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Price = reader.GetDecimal(3)
                    };
                    list.Add(report);
                    Debug.WriteLine($"Read record: {report.ProductName}, {report.Category}, {report.Quantity}, {report.Price}");
                }
            }
            finally { Disconnect(); }
            return list;
        }
    }
}
