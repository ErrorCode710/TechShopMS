using TechShopMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace TechShopMS.Services
{
    public class SaleDbManager: BaseDbManager
    {
        public SaleDbManager(string connectionString) : base(connectionString) { }

        public void AddSale(Sale sale)
        {
            Connect();
            using var transaction = _connection.BeginTransaction();

            try
            {
                // Insert into Sales table
                string saleQuery = @"INSERT INTO Sale 
            (CustomerId, InvoiceNumber, SaleDate, TotalAmount, AmountPaid, Change) 
            VALUES 
            (@CustomerId, @InvoiceNumber, @SaleDate, @TotalAmount, @AmountPaid, @Change);
            SELECT last_insert_rowid();";

                using var saleCommand = new SqliteCommand(saleQuery, _connection, transaction);
                saleCommand.Parameters.AddWithValue("@CustomerId", sale.CustomerId);
                saleCommand.Parameters.AddWithValue("@InvoiceNumber", sale.InvoiceNumber);
                saleCommand.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                saleCommand.Parameters.AddWithValue("@TotalAmount", sale.TotalAmount);
                saleCommand.Parameters.AddWithValue("@AmountPaid", sale.AmountPaid);
                saleCommand.Parameters.AddWithValue("@Change", sale.Change);

                long saleId = (long)(saleCommand.ExecuteScalar() ?? 0);

                // Insert each SaleItem
                foreach (var item in sale.Items)
                {
                    string itemQuery = @"INSERT INTO SaleItem 
                (SalesId, ProductName, Quantity, UnitPrice, Discount, Description)
                VALUES 
                (@SalesId, @ProductName, @Quantity, @UnitPrice, @Discount, @Description)";

                    using var itemCommand = new SqliteCommand(itemQuery, _connection, transaction);
                    itemCommand.Parameters.AddWithValue("@SalesId", saleId);
                    itemCommand.Parameters.AddWithValue("@ProductName", item.ProductName);
                    itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                    itemCommand.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                    itemCommand.Parameters.AddWithValue("@Discount", item.Discount);
                    itemCommand.Parameters.AddWithValue("@Description", item.Description);

                    itemCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Debug.WriteLine($"[AddSale] ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public List<Sale> GetSalesByCustomer(int customerId)
        {
            var sales = new List<Sale>();

            Connect();

            try
            {
                string saleQuery = @"SELECT * FROM Sale WHERE CustomerId = @custId ORDER BY datetime(SaleDate) DESC";

                using var saleCommand = new SqliteCommand(saleQuery, _connection);
                saleCommand.Parameters.AddWithValue("@custId", customerId);

                using var reader = saleCommand.ExecuteReader();
                while (reader.Read())
                {
                    var sale = new Sale
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        InvoiceNumber = reader["InvoiceNumber"].ToString(),
                        SaleDate = DateTime.Parse(reader["SaleDate"].ToString() ?? ""),
                      
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        
                        AmountPaid = Convert.ToDecimal(reader["AmountPaid"]),
                        Change = Convert.ToDecimal(reader["Change"]),
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Items = new List<SaleItem>()
                    };

                    // Load SaleItems for this sale
                    string itemQuery = @"SELECT * FROM SaleItem WHERE SalesId = @saleId";
                    using var itemCommand = new SqliteCommand(itemQuery, _connection);
                    itemCommand.Parameters.AddWithValue("@saleId", sale.Id);

                    using var itemReader = itemCommand.ExecuteReader();
                    while (itemReader.Read())
                    {
                        var item = new SaleItem
                        {
                            Id = Convert.ToInt32(itemReader["Id"]),
                            ProductName = itemReader["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(itemReader["Quantity"]),
                            UnitPrice = Convert.ToDecimal(itemReader["UnitPrice"]),
                            Discount = Convert.ToDecimal(itemReader["Discount"]),
                            Description = itemReader["Description"].ToString()
                        };

                        sale.Items.Add(item);
                    }

                    sales.Add(sale);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetSalesByCustomer] ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                Disconnect();
            }

            return sales;
        }

        public List<SalesReport> GetSalesReport()
        {
            Connect();
            var list = new List<SalesReport>();
            try
            {
                // Check if the CustomerDB is already attached
                using var checkCmd = new SqliteCommand("PRAGMA database_list;", _connection);
                using var checkReader = checkCmd.ExecuteReader(); // Renamed this reader

                bool isCustomerDbAttached = false;

                while (checkReader.Read())
                {
                    if (checkReader["name"].ToString() == "CustomerDB")
                    {
                        isCustomerDbAttached = true;
                        break;
                    }
                }

                // Attach the database only if it's not already attached
                if (!isCustomerDbAttached)
                {
                    using var attachCmd = new SqliteCommand("ATTACH DATABASE 'Data/CustomerDB.db' AS CustomerDB;", _connection);
                    attachCmd.ExecuteNonQuery();
                }

                string query = @"
SELECT s.InvoiceNumber,
       c.FirstName || ' ' || c.LastName AS CustomerName,
       s.SaleDate,
       s.TotalAmount
FROM Sale s
JOIN CustomerDB.Customer c ON s.CustomerId = c.Id";

                Debug.WriteLine("Executing query: " + query);

                using var cmd = new SqliteCommand(query, _connection);
                using var reader = cmd.ExecuteReader(); // This is now the only `reader` in this scope

                int recordCount = 0;

                while (reader.Read())
                {
                    var report = new SalesReport
                    {
                        InvoiceNumber = reader.GetString(0),
                        CustomerName = reader.GetString(1),
                        SaleDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                    list.Add(report);
                    recordCount++;

                    Debug.WriteLine($"Read record: InvoiceNumber={report.InvoiceNumber}, CustomerName={report.CustomerName}, SaleDate={report.SaleDate}, TotalAmount={report.TotalAmount}");
                }

                Debug.WriteLine($"Total records retrieved: {recordCount}");
                Debug.WriteLine("Sales report generated successfully.");
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetSalesReport] ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public void DeleteCustomerSales(int customerId)
        {
            Connect();
            using var transaction = _connection.BeginTransaction();

            try
            {
                // First delete all sale items for the customer's sales
                string deleteItemsQuery = @"
            DELETE FROM SaleItem 
            WHERE SalesId IN (SELECT Id FROM Sale WHERE CustomerId = @customerId)";

                using var itemsCommand = new SqliteCommand(deleteItemsQuery, _connection, transaction);
                itemsCommand.Parameters.AddWithValue("@customerId", customerId);
                itemsCommand.ExecuteNonQuery();

                // Then delete all sales for the customer
                string deleteSalesQuery = "DELETE FROM Sale WHERE CustomerId = @customerId";
                using var salesCommand = new SqliteCommand(deleteSalesQuery, _connection, transaction);
                salesCommand.Parameters.AddWithValue("@customerId", customerId);
                salesCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Debug.WriteLine($"[DeleteCustomerSales] ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                Disconnect();
            }
        }



    }
}
