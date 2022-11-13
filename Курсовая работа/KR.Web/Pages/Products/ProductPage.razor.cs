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
using BlazorDownloadFile;

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
        [Inject]
        private IBlazorDownloadFileService blazorDownloadFileService { get; set; }

        IEnumerable<ProductStorage> getProductResult;
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
                await file.OpenReadStream(5120000000).CopyToAsync(ms);
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