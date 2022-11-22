using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;

namespace KR.Web.Pages.PurchaseAgreements
{
    public partial class PurchaseAgreementPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<PurchaseAgreement>? grid;
        [Inject]
        private PurchaseAgreementService PurchaseAgreementService { get; set; }

        private IEnumerable<PurchaseAgreement> getAreementsResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getAreementsResult = await PurchaseAgreementService.GetPurchaseAgreements();
        }

        private async Task AddPurchaseAgreement()
        {
            await DialogService.OpenAsync<AddPurchaseAgreement>(ConstantValues.ADD_AGREEMENT);
            PurchaseAgreementService.Reload();
            await grid.Reload();
        }

        private async Task DeleteAgreement(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!PurchaseAgreementService.DeletePurchaseAgreement(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    PurchaseAgreementService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task PurchaseAgreementRowSelect(PurchaseAgreement args)
        {
            await DialogService.OpenAsync<EditPurchaseAgreement>(ConstantValues.EDIT_AGREEMENT, new Dictionary<string, object>()
            {{ConstantValues.Id_Purchase_Agreement, args.Id_Purchase_Agreement}});
            PurchaseAgreementService.Reload();
            await grid.Reload();
        }
    }
}