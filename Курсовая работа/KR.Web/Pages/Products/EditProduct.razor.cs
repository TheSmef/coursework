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

namespace KR.Web.Pages.Products
{
    public partial class EditProduct
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private CategoryService CategoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public dynamic Id_Product { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private List<Category> categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            product = await ProductService.GetProductById(Id_Product);
            if (product == null)
            {
                await Close(null);
                return;
            }
            categories = (await CategoryService.GetCategories()).ToList();
        }

        private ProductStorage product = new ProductStorage();
        private async Task HandleUserCreation()
        {
            try
            {
                ProductStorage? obj = await ProductService.GetProductByIdToReadOnly(Id_Product);
                if (!ProductService.CheckProductName(product.Name) && product.Name != obj.Name)
                {
                    Error = ConstantValues.ERROR_PRODUCT_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }

                await ProductService.EditProduct(product);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_PRODUCT_EDIT;
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