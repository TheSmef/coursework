using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Security;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Profile
{
    public partial class Profile
    {
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        protected UserService UserService { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }


        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        public UserData CurrentUser { get; set; }


        private EditUserModel account = new EditUserModel();
        private User user = new User();
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }


        private async Task Load()
        {
            try
            {
                CurrentUser = await LocalStorage.GetItemAsync<UserData>(ConstantValues.USER_LOCATION);
                User? userCheck = await UserService.GetUserById(CurrentUser.UserId); 
                if (userCheck == null)
                {
                    await AuthStateProvider.GetAuthenticationStateAsync();
                    NavigationManager.NavigateTo("login");
                    return;
                }
                user = userCheck;
                account.Last_name = user.Last_name;
                account.First_name = user.First_name;
                account.Otch = user.Otch;
                account.BirthDate = user.BirthDate;
                account.Email = user.Account.Email;
                account.Login = user.Account.Login;
                account.Roles = user.Account.Roles.ToList();
                if (account == null)
                {
                    await AuthStateProvider.GetAuthenticationStateAsync();
                    NavigationManager.NavigateTo("login");
                    return;
                }
            }
            catch
            {
                await AuthStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("login");
                return;
            }

        }

        private async Task HandleEdit()
        {
            try
            {
                User? obj = await UserService.GetUserByIdToReadOnly(CurrentUser.UserId);
                if (obj == null)
                {
                    Error = ConstantValues.ERROR_PROFILE_EDIT;
                    HaveErrors = true;
                    return;
                }

                if (!UserService.CheckUserEmail(account.Email) && account.Email != obj.Account.Email)
                {
                    Error = ConstantValues.ERROR_REGISTER_EMAIL;
                    HaveErrors = true;
                    return;
                }

                if (!UserService.CheckUserLogin(account.Login) && account.Login != obj.Account.Login)
                {
                    Error = ConstantValues.ERROR_REGISTER_LOGIN;
                    HaveErrors = true;
                    return;
                }

                user.First_name = account.First_name;
                user.Last_name = account.Last_name;
                user.Otch = account.Otch;
                user.BirthDate = account.BirthDate;
                user.Account.Email = account.Email;
                user.Account.Login = account.Login;
                user.Account.Roles = account.Roles;
                if (!string.IsNullOrEmpty(account.Password))
                {
                    user.Account.Password = HashProvider.MakeHash(account.Password);
                }

                await UserService.EditUser(user);
                UserData userData = new UserData();
                userData.UserId = user.Id_User;
                userData.Password = user.Account.Password;
                userData.Login = user.Account.Login;
                await LocalStorage.SetItemAsync<UserData>(ConstantValues.USER_LOCATION, userData);
                await AuthStateProvider.GetAuthenticationStateAsync();
                await DialogService.OpenAsync<SuccessDialog>(ConstantValues.SUCCESS);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_PROFILE_EDIT;
                HaveErrors = true;
            }
        }
    }
}