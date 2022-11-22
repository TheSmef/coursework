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
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;

namespace KR.Web.Pages.Statistics
{
    public partial class ProductSalesStats
    {
        private DateTime dateFrom = DateTime.Now.AddMonths(-3);
        private DateTime dateTo = DateTime.Now;
        private List<SalesStats> sales = new List<SalesStats>();
        [Inject]
        private StatsService StatsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            sales = await StatsService.GetStats(dateFrom, dateTo);
        }
    }
}