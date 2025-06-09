using Avalonia.Controls;
using TechShopMS.Services;
using TechShopMS.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Diagnostics;
using System.Threading.Tasks;



namespace TechShopMS.ViewModels
{
  
    public partial class LogInWindowViewModel 
    {
        private readonly UserDbManager _userDbManager;
        private readonly Window _loginWindow;


        public string UserName { get; set; } = "Admin";
        public string Password { get; set; } = "Admin123";



        public LogInWindowViewModel(Window loginWindow)
        {
            _loginWindow = loginWindow;
            _userDbManager = new UserDbManager("Data Source=data/UserDB.db");
        }

       public void OnLogInButtonClick()
        {

            //logInValidationAsync(); 
            onLoginSuccess();

        }

        private async Task logInValidationAsync()
        {
            Debug.WriteLine($"UserName: {UserName}");
            Debug.WriteLine($"Password: {Password}");
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                

                var box = MessageBoxManager
           .GetMessageBoxStandard("Error", "Username or Password is empty.",
               ButtonEnum.Ok);

                var result = await box.ShowAsync();
            }

            if (_userDbManager.isUserAccountValid(UserName, Password))
            {
                Debug.WriteLine("User account is valid");
                Debug.WriteLine("Return FullName, Email and Role");

                onLoginSuccess();
            }
            else
            {
                var box = MessageBoxManager
          .GetMessageBoxStandard("Error", "Login Invalid Please Try Again",
              ButtonEnum.Ok);



                var result = await box.ShowAsync();

            }


            
        }
        private void onLoginSuccess()
        {

           var user =  _userDbManager.UserDisplayDetails(UserName, Password);

            string fullname = $"{user.FirstName} {user.LastName}";
            string email = user.Email;
            string role = $"{user.Role}";

            Debug.WriteLine($"{fullname}, {email}, {role}");
           
            SessionService.CurrentRole = role;
            SessionService.CurrentUser = fullname;

            var mainWindow = new MainWindow();
            var mainWindowVm = new MainWindowViewModel
            {
                FullName = fullname,
                Email = email,
                UserRoleDisplay = role
            };
            
            mainWindow.DataContext = mainWindowVm;

            mainWindow.Show();
            mainWindowVm.SetUpNavigationItems();
            _loginWindow.Close();


        }
        public void init()
        {
           
            _userDbManager.ListUsers();
        }
    }
}
