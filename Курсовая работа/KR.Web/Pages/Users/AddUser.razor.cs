using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Users
{
    public partial class AddUser
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserService UserService { get; set; }
        [Inject]
        private AuthService AuthService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }


        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private List<Role> roles { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            roles = Role.GetRoles();
        }

        private RegistrationModel account = new RegistrationModel();
        private async Task HandleUserCreation()
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

                if (!UserService.CreateNewAccount(account))
                {
                    Error = ConstantValues.ERROR_REGISTER_UNKNOWN;
                    HaveErrors = true;
                    return;
                }

                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_REGISTER_UNKNOWN;
                HaveErrors = true;
                return;
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }
    }
}