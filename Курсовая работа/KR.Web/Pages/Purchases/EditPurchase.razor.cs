using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Purchases
{
    public partial class EditPurchase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private PurchaseService PurchaseService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public dynamic Id_Purchase { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            try
            {
                Purchase purchaseCheck = await PurchaseService.GetPurchaseById(Id_Purchase);
                if (purchaseCheck == null)
                {
                    await Close(null);
                    return;
                }
                purchase = purchaseCheck;
            }
            catch
            {
                await Close(null);
            }
        }

        private Purchase purchase = new Purchase();
        private async Task HandleEdit()
        {
            try
            {
                await PurchaseService.EditPurchase(purchase);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_EDIT_PURCHASE;
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