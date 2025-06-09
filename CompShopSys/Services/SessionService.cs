using CompShopSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompShopSys.Services
{
    public static class SessionService
    {
        public static string CurrentRole { get; set; }
        public static string CurrentUser { get; set; } // 
    }
}
