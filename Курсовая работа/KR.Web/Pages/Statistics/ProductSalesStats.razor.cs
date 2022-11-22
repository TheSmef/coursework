using Microsoft.AspNetCore.Components;
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