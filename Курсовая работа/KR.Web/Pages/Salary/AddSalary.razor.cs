using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;
using Radzen.Blazor;

namespace KR.Web.Pages.Salary
{
    public partial class AddSalary
    {
        [Inject]
        private UserPostService UserPostService { get; set; }

        [Inject]
        private SalaryHistoryService SalaryHistoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDropDownDataGrid<UserPost>? grid;
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<UserPost> users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            users = (await UserPostService.GetUserPosts()).ToList();
            grid?.Reload();
        }

        private SalaryHistory salaryHistory = new SalaryHistory();
        private async Task HandleAdd()
        {
            try
            {
                await SalaryHistoryService.AddSalaryHistory(salaryHistory);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_ADD_SALARY;
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