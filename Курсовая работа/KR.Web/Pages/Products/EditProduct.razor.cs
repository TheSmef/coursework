using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

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