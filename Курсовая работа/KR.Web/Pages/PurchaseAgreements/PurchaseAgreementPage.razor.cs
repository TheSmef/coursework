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
using KR.Web.Services;
using Kr.Models;
using System.ComponentModel;

namespace KR.Web.Pages.PurchaseAgreements
{
    public partial class PurchaseAgreementPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<PurchaseAgreement>? grid;
        [Inject]
        private PurchaseAgreementService PurchaseAgreementService { get; set; }

        IEnumerable<PurchaseAgreement> getAreementsResult;
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
            await DialogService.OpenAsync<AddPurchaseAgreemen>(ConstantValues.ADD_PRODUCT);
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
            await DialogService.OpenAsync<EditPurchaseAgreemen>(ConstantValues.EDIT_AGREEMENT, new Dictionary<string, object>()
            {{ConstantValues.Id_Purchase_Agreement, args.Id_Purchase_Agreement}});
            PurchaseAgreementService.Reload();
            await grid.Reload();
        }
    }
}