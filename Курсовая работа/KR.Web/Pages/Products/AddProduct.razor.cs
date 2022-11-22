using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Products
{
    public partial class AddProduct
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private CategoryService CategoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private List<Category> categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            categories = (await CategoryService.GetCategories()).ToList();
        }

        private ProductStorage product = new ProductStorage();
        private async Task HandleAdd()
        {
            try
            {
                if (!ProductService.CheckProductName(product.Name))
                {
                    Error = ConstantValues.ERROR_PRODUCT_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }

                await ProductService.AddProduct(product);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_PRODUCT_ADD;
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