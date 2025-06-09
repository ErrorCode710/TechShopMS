using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using TechShopMS.Models;
using System.Diagnostics;

namespace TechShopMS.Services
{
   public  class CustomerDbManager: BaseDbManager
    {
        private readonly SaleDbManager _saleDbManager;
        public CustomerDbManager(string connectionString) : base(connectionString) {
            _saleDbManager = new SaleDbManager("Data Source=data/SalesDB.db");
        }

        public List<Customer> GetAllCustomers()
        {
            Connect(); 
            try
            {
                Debug.WriteLine("Get All Customer Is Running");
                var customers = new List<Customer>();
                string query = "SELECT * FROM Customer";
                using var command = new SqliteCommand(query, _connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        ContactNumber = reader.GetString(3),
                        Email = reader.GetString(4),
                        Address = reader.GetString(7),
                        DateCreated = DateTime.TryParse(reader.GetString(5), out DateTime dateCreated)? dateCreated: DateTime.MinValue,
                        IsActive = reader.GetInt32(6) == 1
                    });
                }
                return customers;
            }
            finally
            {
                Disconnect(); 
            }
        }
        public void AddCustomer(Customer customer)
        {

            Connect();
            try
            {
                string query = "INSERT INTO Customer (FirstName, LastName, ContactNumber, Email, Address, DateCreated, IsActive) " +
                               "VALUES (@FirstName, @LastName, @ContactNumber, @Email, @Address, @DateCreated, @IsActive)";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@ContactNumber", customer.ContactNumber);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@DateCreated", customer.DateCreated.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@IsActive", customer.IsActive ? 1 : 0);

                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }

        public void RemoveCustomer(int Id)
        {
            Connect();
            try
            {
                string query = "DELETE FROM Customer WHERE Id = @Id";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }
        
        public void UpdateCustomer(Customer customer)
        {
            Connect();
            try
            {
                string query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, ContactNumber = @ContactNumber, " +
                               "Email = @Email, Address = @Address, DateCreated = @DateCreated, IsActive = @IsActive WHERE Id = @Id";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@ContactNumber", customer.ContactNumber);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@DateCreated", customer.DateCreated.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@IsActive", customer.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@Id", customer.Id);
                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }

        public void DeleteCustomerWithSales(int customerId)
        {
            // First delete sales from the sales database
            try
            {
                _saleDbManager.DeleteCustomerSales(customerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeleteCustomerWithSales] Failed to delete sales: {ex.Message}");
                throw new Exception("Failed to delete customer sales records", ex);
            }

            // Then delete from customer database
            SqliteTransaction customerTransaction = null;

            try
            {
                Connect(); // This sets _connection
                customerTransaction = _connection.BeginTransaction();

                string deleteCustomerQuery = "DELETE FROM Customer WHERE Id = @customerId";
                using var customerCommand = new SqliteCommand(deleteCustomerQuery, _connection, customerTransaction);
                customerCommand.Parameters.AddWithValue("@customerId", customerId);
                customerCommand.ExecuteNonQuery();

                customerTransaction.Commit();
            }
            catch (Exception ex)
            {
                customerTransaction?.Rollback();
                Debug.WriteLine($"[DeleteCustomerWithSales] ERROR: {ex.Message}");
                throw new Exception("Failed to delete customer", ex);
            }
            finally
            {
                customerTransaction?.Dispose();
                Disconnect(); // Use your existing Disconnect() method
            }
        }
    }
}
