using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
using KR.Web.Services;

namespace KR.Web.Pages.Products
{
    public partial class ProductPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<ProductStorage>? grid;
        [Inject]
        private ProductService ProductService { get; set; }
        [Inject]
        private ExportService ExportService { get; set; }

        private IEnumerable<ProductStorage> getProductResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getProductResult = await ProductService.GetProducts();
        }

        private async Task AddProduct()
        {
            await DialogService.OpenAsync<AddProduct>(ConstantValues.ADD_PRODUCT);
            ProductService.Reload();
            await grid.Reload();
        }

        private async Task DeleteProduct(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!ProductService.DeleteProduct(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    ProductService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task ProductRowSelect(ProductStorage args)
        {
            await DialogService.OpenAsync<EditProduct>(ConstantValues.EDIT_PRODUCT, new Dictionary<string, object>()
            {{ConstantValues.Id_Product, args.Id_Product_Storage}});
            ProductService.Reload();
            await grid.Reload();
        }

        private async void UploadFile(IBrowserFile file)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(file.Size).CopyToAsync(ms);
                byte[] data = ms.ToArray();
                ExportService.ImportProductsFromCvs(data);
                grid.Reload();
            }
            catch
            {
                await DialogService.OpenAsync<ErrorImport>(ConstantValues.IMPORT_ERROR);
            }
        }

        private async Task GetDoc()
        {
            try
            {
                await ExportService.ExportProductsToCsv(grid.View.ToList());
            }
            catch
            {
                await DialogService.OpenAsync<ErrorExport>(ConstantValues.EXPORT_ERROR);
            }

        }
    }
}