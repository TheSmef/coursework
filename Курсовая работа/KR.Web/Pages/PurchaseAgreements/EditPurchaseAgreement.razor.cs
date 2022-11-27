using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.PurchaseAgreements
{
    public partial class EditPurchaseAgreement
    {
        [Parameter]
        public dynamic Id_Purchase_Agreement { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private PurchaseAgreementService PurchaseAgreementService { get; set; }

        [Inject]
        private CategoryService CategoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

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
                PurchaseAgreement purchaseAgreementCheck = await PurchaseAgreementService.GetPurchaseAgreementById(Id_Purchase_Agreement);
                if (purchaseAgreementCheck == null)
                {
                    await Close(null);
                    return;
                }
                purchaseAgreement = purchaseAgreementCheck;
            }
            catch
            {
                await Close(null);
            }

        }

        private PurchaseAgreement purchaseAgreement = new PurchaseAgreement();
        private async Task HandleEdit()
        {
            try
            {
                await PurchaseAgreementService.EditPurchaseAgreement(purchaseAgreement);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_AGREEMENT_EDIT;
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