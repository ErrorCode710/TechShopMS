using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.Models
{
    public class User
    {
        public UserRole Role { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsProtected { get; set; }  
    }
    public enum UserRole
    {
        Staff = 0,     
        Manager = 1,   
        Admin = 2

    }
}
