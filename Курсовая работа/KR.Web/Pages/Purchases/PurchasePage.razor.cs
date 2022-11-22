using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
using KR.Web.Services;

namespace KR.Web.Pages.Purchases
{
    public partial class PurchasePage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<Purchase>? grid;
        [Inject]
        private PurchaseService PurchaseService { get; set; }

        private IEnumerable<Purchase> getPurchaseResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getPurchaseResult = await PurchaseService.GetPurchases();
        }

        private async Task AddPurchase()
        {
            await DialogService.OpenAsync<AddPurchase>(ConstantValues.ADD_PURCHASE);
            PurchaseService.Reload();
            await grid.Reload();
        }

        private async Task DeletePurchase(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!PurchaseService.DeletePurchase(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    PurchaseService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task PurchaseRowSelect(Purchase args)
        {
            await DialogService.OpenAsync<EditPurchase>(ConstantValues.EDIT_PURCHASE, new Dictionary<string, object>()
            {{ConstantValues.Id_Purchase, args.Id_Puchase}});
            PurchaseService.Reload();
            await grid.Reload();
        }
    }
}