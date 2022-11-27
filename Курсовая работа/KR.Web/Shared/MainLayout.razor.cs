using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using KR.Web.Constants;
using Blazored.LocalStorage;

namespace KR.Web.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }


        private RadzenBody? body;
        private RadzenSidebar? sidebar;
        protected async Task SidebarToggleClick(dynamic args)
        {
            await InvokeAsync(() =>
            {
                sidebar.Toggle();
            });
            await InvokeAsync(() =>
            {
                body.Toggle();
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