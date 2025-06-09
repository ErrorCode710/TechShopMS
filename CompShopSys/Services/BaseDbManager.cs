using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.Services
{
   public abstract class BaseDbManager
    {
        private readonly string _connectionString;
        protected SqliteConnection _connection;

        protected BaseDbManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Connect()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            Debug.WriteLine($"Connecting to database with connection string: {_connectionString}");
           
        }
        public void Disconnect()
        {
            if (_connection != null && _connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                Debug.WriteLine("Database connection closed.");
            }
        }
        public void ExecuteQuery(string query)
        {
            using var cmd = new SqliteCommand(query, _connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Executed query: {query}");
        }
    }
}
