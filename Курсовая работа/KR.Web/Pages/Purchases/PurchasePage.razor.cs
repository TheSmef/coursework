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
using Blazored.LocalStorage;
using KR.Web.Constants;
using Kr.Models;
using System.ComponentModel;
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

        IEnumerable<Purchase> getPurchaseResult;
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