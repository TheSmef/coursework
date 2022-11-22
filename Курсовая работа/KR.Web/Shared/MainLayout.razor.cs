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