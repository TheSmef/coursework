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
    public partial class EditPurchase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected PurchaseService PurchaseService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

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
            purchase = await PurchaseService.GetPurchaseById(Id_Purchase);
        }

        Purchase purchase = new Purchase();
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