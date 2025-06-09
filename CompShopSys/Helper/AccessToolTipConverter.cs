using Avalonia.Data.Converters;
using System;
using System.Diagnostics;
using System.Globalization;

namespace TechShopMS.Helper 
{
    public class AccessToolTipConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Debug.WriteLine($"isEnable: {value}");
           
            Debug.WriteLine($"Running");
            if (value is bool isEnabled && !isEnabled)
                return "Access restricted to Admins";

            return string.Empty; 
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
