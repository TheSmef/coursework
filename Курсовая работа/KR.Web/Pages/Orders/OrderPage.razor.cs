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

namespace KR.Web.Pages.Orders
{
    public partial class OrderPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<Order>? grid;
        [Inject]
        private OrderService OrderService { get; set; }

        private IEnumerable<Order> getOrderResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getOrderResult = await OrderService.GetOrders();
        }

        private async Task AddOrder()
        {
            await DialogService.OpenAsync<AddOrder>(ConstantValues.ADD_ORDER);
            OrderService.Reload();
            await grid.Reload();
        }

        private async Task DeleteOrder(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!OrderService.DeleteOrder(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    OrderService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task OrderRowSelect(Order args)
        {
            await DialogService.OpenAsync<EditOrder>(ConstantValues.EDIT_ORDER, new Dictionary<string, object>()
            {{ConstantValues.Id_Order, args.Id_Order}});
            OrderService.Reload();
            await grid.Reload();
        }
    }
}