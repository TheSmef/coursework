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
    public partial class AddPurchaseAgreemen
    {
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

        PurchaseAgreement purchaseAgreement = new PurchaseAgreement();
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