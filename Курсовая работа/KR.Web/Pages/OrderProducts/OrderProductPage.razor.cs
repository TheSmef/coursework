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

namespace KR.Web.Pages.OrderProducts
{
    public partial class OrderProductPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<OrderProduct>? grid;
        [Inject]
        private OrderProductService OrderProductService { get; set; }

        IEnumerable<OrderProduct> getOrderProductResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getOrderProductResult = await OrderProductService.GetOrderProducts();
        }

        private async Task AddOrderProduct()
        {
            await DialogService.OpenAsync<AddOrderProduct>(ConstantValues.ADD_ORDERPRODUCT);
            OrderProductService.Reload();
            await grid.Reload();
        }

        private async Task DeleteOrderProduct(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!OrderProductService.DeleteOrderProduct(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    OrderProductService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task OrderProductRowSelect(OrderProduct args)
        {
            await DialogService.OpenAsync<EditOrderProduct>(ConstantValues.EDIT_ORDERPRODUCT, new Dictionary<string, object>()
            {{ConstantValues.Id_OrderProduct, args.Id_order_product}});
            OrderProductService.Reload();
            await grid.Reload();
        }
    }
}