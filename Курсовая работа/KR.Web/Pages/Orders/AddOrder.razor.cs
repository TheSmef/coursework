using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;
using Radzen.Blazor;

namespace KR.Web.Pages.Orders
{
    public partial class AddOrder
    {
        [Inject]
        private UserPostService UserPostService { get; set; }

        [Inject]
        private OrderService OrderService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDropDownDataGrid<UserPost>? grid;
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<UserPost> users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            users = (await UserPostService.GetUserPosts()).ToList();
            grid?.Reload();
        }

        private Order order = new Order();
        private async Task HandleAdd()
        {
            try
            {
                await OrderService.AddOrder(order);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_USERPOST_ADD;
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