using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using KR.Web;
using KR.Web.Shared;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;
using Kr.Models;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace KR.Web.Pages.PurchaseAgreements
{
    public partial class EditPurchaseAgreemen
    {
        [Parameter]
        public dynamic Id_Purchase_Agreement { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected PurchaseAgreementService PurchaseAgreementService { get; set; }

        [Inject]
        protected CategoryService CategoryService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            purchaseAgreement = await PurchaseAgreementService.GetPurchaseAgreementById(Id_Purchase_Agreement);
            if (purchaseAgreement == null)
            {
                await Close(null);
                return;
            }
        }

        PurchaseAgreement purchaseAgreement = new PurchaseAgreement();
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