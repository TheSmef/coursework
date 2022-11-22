using Microsoft.AspNetCore.Components;
using KR.Web.Models;
using KR.Web.Services;

namespace KR.Web.Pages.Statistics
{
    public partial class PostStats
    {
        private DateTime dateFrom = DateTime.Now.AddMonths(-3);
        private DateTime dateTo = DateTime.Now;
        private List<SpentStats> stats = new List<SpentStats>();
        [Inject]
        private StatsService StatsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            stats = await StatsService.GetBuhStats(dateFrom, dateTo);
        }
    }
}