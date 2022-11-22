using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.OrderProducts
{
    public partial class EditOrderProduct
    {
        [Inject]
        private OrderProductService OrderProductService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public dynamic Id_OrderProduct { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<Order> orders { get; set; }

        private IEnumerable<ProductStorage> products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            orderproduct = await OrderProductService.GetOrderProductById(Id_OrderProduct);
        }

        private OrderProduct orderproduct = new OrderProduct();
        private async Task HandleEdit()
        {
            try
            {
                await OrderProductService.EditOrderProduct(orderproduct);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_EDIT_ORDERPRODUCT;
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