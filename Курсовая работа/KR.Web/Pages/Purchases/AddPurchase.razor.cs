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

namespace KR.Web.Pages.Purchases
{
    public partial class AddPurchase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private PurchaseService PurchaseService { get; set; }

        [Inject]
        private PurchaseAgreementService PurchaseAgreementService { get; set; }

        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<PurchaseAgreement> agreements { get; set; }

        private IEnumerable<ProductStorage> products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            products = (await ProductService.GetProducts()).ToList();
            agreements = (await PurchaseAgreementService.GetPurchaseAgreements()).ToList();
        }

        private Purchase purchase = new Purchase();
        private async Task HandleAdd()
        {
            try
            {
                if (!PurchaseService.CheckPurchase(purchase))
                {
                    Error = ConstantValues.ERROR_PURCHASE_EXISTS;
                    HaveErrors = true;
                    return;
                }

                await PurchaseService.AddPurchase(purchase);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_ADD_PURCHASE;
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