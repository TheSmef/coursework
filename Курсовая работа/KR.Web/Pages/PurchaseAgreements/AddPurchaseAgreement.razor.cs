using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.PurchaseAgreements
{
    public partial class AddPurchaseAgreement
    {
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

        private PurchaseAgreement purchaseAgreement = new PurchaseAgreement();
        private async Task HandleAdd()
        {
            try
            {
                await PurchaseAgreementService.AddPurchaseAgreement(purchaseAgreement);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_AGREEMENT_ADD;
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