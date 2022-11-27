using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Models;
using Radzen;
using KR.Web.Services;
using KR.Web.Constants;
using KR.Web.Security;
using Blazored.LocalStorage;

namespace KR.Web.Pages.Login
{
    public partial class Register
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }
        [Inject]
        protected AuthService AuthService { get; set; }

        private bool HaveErrors { get; set; }
        private string Error { get; set; }

        RegistrationModel account = new RegistrationModel();
        private async Task HandleRegistration()
        {
            try
            {
                if (!AuthService.CheckEmailForUnique(account.Email))
                {
                    Error = ConstantValues.ERROR_REGISTER_EMAIL;
                    HaveErrors = true;
                    return;
                }
                if (!AuthService.CheckLoginForUnique(account.Login))
                {
                    Error = ConstantValues.ERROR_REGISTER_LOGIN;
                    HaveErrors = true;
                    return;
                }
                if (!AuthService.CreateNewAccount(account))
                {
                    Error = ConstantValues.ERROR_REGISTER_UNKNOWN;
                    HaveErrors = true;
                    return;
                }
                string Password = HashProvider.MakeHash(account.Password);
                Guid acc = AuthService.PrepareAuth(account.Login, Password);
                if (!acc.Equals(Guid.Empty))
                {
                    UserData userData = new UserData();
                    userData.UserId = acc;
                    userData.Password = Password;
                    userData.Login = account.Login;
                    await LocalStorage.SetItemAsync<UserData>(ConstantValues.USER_LOCATION, userData);
                    await AuthStateProvider.GetAuthenticationStateAsync();
                    NavigationManager.NavigateTo("profile");
                }
                else
                {
                    Error = ConstantValues.ERROR_REGISTER_UNKNOWN;
                    HaveErrors = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_REGISTER_UNKNOWN;
                HaveErrors = true;
                return;
            }
        }

        protected async Task Close(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}