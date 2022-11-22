using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using KR.Web.Models;
using KR.Web.Security;
using KR.Web.Services;
using Kr.Web.Models;
using Radzen;
using KR.Web.Constants;

namespace KR.Web.Pages.Login
{
    public partial class Login
    {
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }   
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private AuthService AuthService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string? returnUrl { get; set; }

        private AccountModel account = new AccountModel();
        private bool HaveErrors
        {get; set;}
        private string Error { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            if (authenticationState != null)
            {
                if (authenticationState.User.Identity != null)
                {
                    NavigationManager.NavigateTo("");
                }
            }
        }

        private async System.Threading.Tasks.Task HandleLogin()
        {
            try
            {
                string Password = HashProvider.MakeHash(account.Password);
                if (string.IsNullOrEmpty(Password))
                {
                    return;
                }

                Guid acc = AuthService.PrepareAuth(account.Login, Password);
                if (!acc.Equals(Guid.Empty))
                {
                    UserData userData = new UserData();
                    userData.UserId = acc;
                    userData.Password = Password;
                    userData.Login = account.Login;
                    await LocalStorage.SetItemAsync<UserData>(ConstantValues.USER_LOCATION, userData);
                    await AuthStateProvider.GetAuthenticationStateAsync();
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        NavigationManager.NavigateTo($"/{returnUrl}");
                    }
                    else
                    {
                        NavigationManager.NavigateTo("profile");
                    }
                }
                else
                {
                    Error = ConstantValues.ERROR_AUTH_MESSAGE;
                    HaveErrors = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async System.Threading.Tasks.Task ToRegistration()
        {
            var dialogResult = await DialogService.OpenAsync<Register>(ConstantValues.REGISTRATION, null);
        }


    }
}