using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using KR.Web;
using KR.Web.Shared;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Blazored.LocalStorage;

namespace KR.Web.Shared
{
    public partial class MainLayout
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }
        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; }


        protected RadzenBody? body0;
        protected RadzenSidebar? sidebar0;
        protected async System.Threading.Tasks.Task SidebarToggle0Click(dynamic args)
        {
            await InvokeAsync(() =>
            {
                sidebar0.Toggle();
            });
            await InvokeAsync(() =>
            {
                body0.Toggle();
            });
        }

        private async void NavMenuClicked(dynamic args)
        {
            if (args.Value == 1)
            {
                await LocalStorage.RemoveItemAsync(ConstantValues.USER_LOCATION);
                await AuthStateProvider.GetAuthenticationStateAsync();
            }
 
        }
    }
}