using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using KR.Web;
using KR.Web.Shared;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;
using Kr.Models;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace KR.Web.Pages.Users
{
    public partial class AddUser
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected UserService UserService { get; set; }
        [Inject]
        protected AuthService AuthService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }


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

        RegistrationModel account = new RegistrationModel();
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