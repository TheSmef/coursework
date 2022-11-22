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

namespace KR.Web.Pages.OrderProducts
{
    public partial class AddOrderProduct
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private OrderProductService OrderProductService { get; set; }

        [Inject]
        private OrderService OrderService { get; set; }

        [Inject]
        private ProductService ProductService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDropDownDataGrid<Order>? grid;
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<Order> orders { get; set; }

        private IEnumerable<ProductStorage> products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            products = (await ProductService.GetProducts()).ToList();
            orders = (await OrderService.GetOrders()).ToList();
            grid?.Reload();
        }

        private OrderProduct orderproduct = new OrderProduct();
        private async Task HandleAdd()
        {
            try
            {
                if (!OrderProductService.CheckOrderProduct(orderproduct))
                {
                    Error = ConstantValues.ERROR_ORDERPRODUCT_EXISTS;
                    HaveErrors = true;
                    return;
                }

                await OrderProductService.AddOrderProduct(orderproduct);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_ADD_ORDERPRODUCT;
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