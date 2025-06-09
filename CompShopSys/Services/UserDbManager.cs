using TechShopMS.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShopMS.Services
{
    public class UserDbManager : BaseDbManager
    {
        public UserDbManager(string connectionString) : base(connectionString) { }

        public bool isUserAccountValid(string UserName, string Password)
        {
            Connect();
            try
            {
                string query = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName AND Password = @Password;";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);

                var result = command.ExecuteScalar();
                return Convert.ToInt32(result) > 0;
            }
            finally
            {
                Disconnect();
            }
        }

        public void AddUser(string firstName, string lastName, string middleName, string email, string userName, string password, int role, bool isProtected = false)
        {
            Connect();
            try
            {
                string query = "INSERT INTO Users (FirstName, LastName, MiddleName, Email, UserName, Password, Role, IsProtected) " +
                               "VALUES (@FirstName, @LastName, @MiddleName, @Email, @UserName, @Password, @Role, @IsProtected)";

                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@MiddleName", middleName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@IsProtected", isProtected ? 1 : 0);

                command.ExecuteNonQuery();
                Debug.Write($"Added User {userName}");
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                Debug.Write($"Duplicate entry detected: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        public void UpdateUser(int id, string firstName, string lastName, string middleName, string email, string username, string password, int role, bool isProtected = false)
        {
            Connect();
            try
            {
                Debug.WriteLine($"Database Updated: Username:{username}");
                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, Email = @Email, UserName = @UserName, Password = @Password, Role = @Role, IsProtected = @IsProtected WHERE Id = @Id";


                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@IsProtected", isProtected ? 1 : 0);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }

        public void DeleteUser(int id)
        {
            Connect();
            try
            {
                string query = "DELETE FROM Users WHERE Id = @Id";
                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            finally
            {
                Disconnect();
            }
        }

        public void dummyData()
        {
            AddUser("Joshua", "Garcia", "Canasa", "garcia@example.com", "garciajoshuae", "4143", 1, true);
            AddUser("Daniel", "Padilla", null, "supremoDj@gmail.com", "supremo_dp", "4143", 0, false);
        }

        public List<User> GetAllUsers()
        {
            Connect();
            try
            {
                var users = new List<User>();
                string query = "SELECT * FROM Users";

                using var command = new SqliteCommand(query, _connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
                        UserName = reader.GetString(4),
                        Password = reader.GetString(5),
                        Email = reader.GetString(6),
                        Role = (UserRole)reader.GetInt32(7),
                        IsProtected = reader.GetInt32(8) == 1
                    });
                }

                return users;
            }
            finally
            {
                Disconnect();
            }
        }

        public User UserDisplayDetails(string userName, string password)
        {
            Connect();
            try
            {
                string query = @"SELECT FirstName, LastName, Email, Role FROM Users WHERE UserName = @UserName AND Password = @Password LIMIT 1;";

                using var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Role = (UserRole)reader.GetInt32(3)
                    };
                }

                return null;
            }
            finally
            {
                Disconnect();
            }
        }

        public void ListUsers()
        {
            Connect();
            try
            {
                string query = "SELECT Id, FirstName, LastName, Email, UserName, Password, Role, IsProtected FROM Users";

                using var command = new SqliteCommand(query, _connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var userName = reader.GetString(4);
                    var password = reader.GetString(5);
                    var role = reader.GetInt32(6);
                    var isProtected = reader.GetInt32(7) == 1;

                    Debug.WriteLine($"ID: {id}, Name: {firstName} {lastName}, Email: {email}, UserName: {userName}, Password: {password}, Role: {role}, IsProtected: {isProtected}");
                }
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
