using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechShopMS.Models;
using TechShopMS.Services;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Styling;
using System.ComponentModel.DataAnnotations;

namespace TechShopMS.ViewModels
{
    public partial class UserManagementPageViewModel : ViewModelBase
    {

        // TODO:
        // ADD CURRENTPAGE PAGINATION WHEN ADDING USER (done)
        // ADD CONSTRAINS IF USER DOESNT HAVE ACCOUNT ADD ADMIN
        // ADD FORGOT ACCOUNT ON THE LOG IN
        #region Fields

        private readonly UserDbManager _dbManager;
        private const int PageSize = 5;

        #endregion

        #region Observable Properties

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private ObservableCollection<User> filteredUsers = new();

        [ObservableProperty]
        private ObservableCollection<User> pagedUser = new();

        [ObservableProperty]
        private User selectedUser;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int totalPages;

        // User form inputs
        [ObservableProperty] private int id;
        [ObservableProperty] private string firstName;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string middleName;
        [ObservableProperty] private string userName;
        [ObservableProperty] private string password;
        [ObservableProperty] private string email;
        [ObservableProperty] private UserRole role;
        [ObservableProperty] private bool isProtected;


        #endregion

        #region Constructor

        public ObservableCollection<UserRole> Roles { get; } = new(
            Enum.GetValues(typeof(UserRole)).Cast<UserRole>());

        public UserManagementPageViewModel()
        {
            _dbManager = new UserDbManager("Data Source=data/UserDB.db");
            LoadUsers();
        }

        #endregion

        #region Pagination

        private void ApplyPagination()
        {
            if (FilteredUsers == null || FilteredUsers.Count == 0)
            {
                PagedUser = new ObservableCollection<User>();
                TotalPages = 1;
                return;
            }

            TotalPages = (int)Math.Ceiling((double)FilteredUsers.Count / PageSize);

            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);

            var paged = FilteredUsers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedUser = new ObservableCollection<User>(paged);
        }
        private void LoadPage(int pageNumber)
        {
            var pageItems = Users
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedUser = new ObservableCollection<User>(pageItems);
        }
        [RelayCommand]
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                //ApplyPagination();
                LoadPage(CurrentPage);
            }
        }

        [RelayCommand]
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadPage(CurrentPage);
                //ApplyPagination();
            }
        }

        #endregion

        #region Search

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredUsers = new ObservableCollection<User>(Users);
            }
            else
            {
                string lowerQuery = SearchQuery.Trim().ToLower();

                var filtered = Users.Where(u =>
                    (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(u.UserName) && u.UserName.ToLower().Contains(lowerQuery)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(lowerQuery)) ||
                    u.Role.ToString().ToLower().Contains(lowerQuery)
                );

                FilteredUsers = new ObservableCollection<User>(filtered);
            }

            CurrentPage = 1;
            ApplyPagination();
        }

        partial void OnSearchQueryChanged(string value)
        {
            Search();
        }

        #endregion

        #region Load & Reset

        private void LoadUsers()
        {
            var allUsers = _dbManager.GetAllUsers();
            Users = new ObservableCollection<User>(allUsers);
            FilteredUsers = new ObservableCollection<User>(Users);
            TotalPages = (int)Math.Ceiling((double)Users.Count / PageSize);
            CurrentPage = 1;
            //ApplyPagination();
            LoadPage(CurrentPage);
        }

        [RelayCommand]
        private void ClearUserForm() => ResetInputFields();

        private void ResetInputFields()
        {
            Id = 0;
            FirstName = LastName = MiddleName = Email = UserName = Password = string.Empty;
            Role = default;
            SelectedUser = null;
        }

        #endregion

        #region Validation

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FirstName) && string.IsNullOrWhiteSpace(FirstName))
                    return "First Name cannot be empty.";
                if (columnName == nameof(LastName) && string.IsNullOrWhiteSpace(LastName))
                    return "Last Name cannot be empty.";
                if (columnName == nameof(UserName) && string.IsNullOrWhiteSpace(UserName))
                    return "Username cannot be empty.";
                if (columnName == nameof(Email) && string.IsNullOrWhiteSpace(Email))
                    return "Email cannot be empty.";
                if (columnName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
                    return "Password cannot be empty.";
                return string.Empty;
            }
        }

        public string Error => "Please ensure all fields are filled correctly.";

        private string GetValidationErrors()
        {
            var sb = new StringBuilder();
            foreach (var prop in new[] { nameof(FirstName), nameof(LastName), nameof(UserName), nameof(Email), nameof(Password) })
            {
                var error = this[prop];
                if (!string.IsNullOrWhiteSpace(error))
                    sb.AppendLine(error);
            }
            return sb.ToString();
        }

        #endregion

        #region User Manipulation

        [RelayCommand]
        private async Task SubmitAsync()
        {
            Debug.WriteLine("CLICKED!");
            var validationErrors = GetValidationErrors();
            if (validationErrors.Length > 0)
            {
                await ShowMessage("Validation Error", validationErrors);
                return;
            }

            if (Users.Any(u => u.UserName.Equals(UserName, StringComparison.OrdinalIgnoreCase)))
            {
                await ShowMessage("Duplicate Username", $"The username '{UserName}' is already taken.");
                return;
            }

            if (Users.Any(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase)))
            {
                await ShowMessage("Duplicate Email", $"The email '{Email}' is already taken.");
                return;
            }

            try
            {
                int numericRole = (int)Role;
                var newUser = new User
                {

                    FirstName = FirstName,
                    LastName = LastName,
                    MiddleName = MiddleName,
                    Email = Email,
                    UserName = UserName,
                    Password = Password,
                    Role = Role
                };
                filteredUsers.Add(newUser);
                _dbManager.AddUser(FirstName, LastName, MiddleName, Email, UserName, Password, numericRole);
                LoadUsers();
                //FilteredUsers.Add(new User
                //{
                //    FirstName = FirstName,
                //    LastName = LastName,
                //    MiddleName = MiddleName,
                //    Email = Email,
                //    UserName = UserName,
                //    Password = Password,
                //    Role = Role
                //});



                
                int lastPage = (int)Math.Ceiling((double)Users.Count / PageSize);
                CurrentPage = lastPage;
                
                ResetInputFields();
                LoadPage(CurrentPage);
                await ShowMessage("Account Created", $"New user account for '{UserName}' has been added on Page {CurrentPage}.");
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"An error occurred: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task EditUserAsync()
        {
            if (SelectedUser is null) return;
            Debug.WriteLine($"Editing User: {SelectedUser.IsProtected}");
            if (SelectedUser.IsProtected)
            {
                await ShowMessage("Protected User", "Cannot Edit a protected user. Contact Higher Authority");
                return;
            }

            var errors = GetValidationErrors();
            if (!string.IsNullOrEmpty(errors))
            {
                await ShowMessage("Validation Error", errors);
                return;
            }

            bool isUsernameDuplicate = Users.Any(u =>
                u.Id != SelectedUser.Id &&
                u.UserName.Equals(UserName, StringComparison.OrdinalIgnoreCase));

            if (isUsernameDuplicate)
            {
                await ShowMessage("Duplicate Username", $"The username '{UserName}' is already taken.");
                return;
            }

            bool isEmailDuplicate = Users.Any(u =>
                u.Id != SelectedUser.Id &&
                u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

            if (isEmailDuplicate)
            {
                await ShowMessage("Duplicate Email", $"The email '{Email}' is already taken.");
                return;
            }

            try
            {
                int numericRole = (int)Role;
                Debug.WriteLine($"Updating User: {Id}, {FirstName}, {LastName}, {MiddleName}, {Email}, {UserName}, {Password}, {numericRole}");
                _dbManager.UpdateUser(Id, FirstName, LastName, MiddleName, Email, UserName, Password, numericRole);

                var index = Users.IndexOf(SelectedUser);
                if (index >= 0)
                {
                    Users[index] = new User
                    {
                        Id = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        MiddleName = MiddleName,
                        Email = Email,
                        UserName = UserName,
                        Password = Password,
                        Role = Role,
                        IsProtected = IsProtected, // Recently add
                    };
                }

                await ShowMessage("Updated", "User updated successfully.");
                ResetInputFields();
                LoadUsers();
            }
            catch (Exception ex)
            {
                await ShowMessage("Error", $"Update failed: {ex.Message}");
            }
        }

        
        [RelayCommand]
        private async Task DeleteUserAsync()
        {
            if (SelectedUser is null)
                return;
            if (SelectedUser.IsProtected) {
                await ShowMessage("Protected User", "Cannot delete a protected user. Contact Higher Authority");
                return;
            }
            var confirmBox = MessageBoxManager.GetMessageBoxStandard(
                "Confirm Delete",
                $"Are you sure you want to delete user '{SelectedUser.UserName}'?",
                ButtonEnum.YesNo,
                Icon.Warning);

            var result = await confirmBox.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                if(SelectedUser.IsProtected)
                {
                    await ShowMessage("Protected User", "Cannot delete a protected user.");
                    return;
                }
                try
                {
                    _dbManager.DeleteUser(SelectedUser.Id);
                    Users.Remove(SelectedUser);
                    ResetInputFields();
                    SelectedUser = null;

                    var infoBox = MessageBoxManager.GetMessageBoxStandard("Deleted", "User deleted successfully.", ButtonEnum.Ok);
                    await infoBox.ShowAsync();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error while deleting: {ex.Message}");
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to delete user. {ex.Message}", ButtonEnum.Ok);
                    await errorBox.ShowAsync();
                }
            }
        }
        #endregion

        #region Helpers

        private async Task ShowMessage(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok);
            await box.ShowAsync();
        }
        partial void OnSelectedUserChanged(User value)
        {
            if (value is not null)
            {
                Id = value.Id;
                FirstName = value.FirstName;
                LastName = value.LastName;
                MiddleName = value.MiddleName;
                Email = value.Email;
                UserName = value.UserName;
                Password = value.Password;
                Role = value.Role;
                isProtected = value.IsProtected;
            }
        }

        #endregion
    }
}
