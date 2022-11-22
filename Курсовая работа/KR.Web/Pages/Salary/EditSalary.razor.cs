using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Salary
{
    public partial class EditSalary
    {
        [Parameter]
        public dynamic Id_SalaryHistory { get; set; }

        [Inject]
        private SalaryHistoryService SalaryHistoryService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            salaryHistory = await SalaryHistoryService.GetSalaryHistoryById(Id_SalaryHistory);
        }

        private SalaryHistory salaryHistory = new SalaryHistory();
        private async Task HandleEdit()
        {
            try
            {
                await SalaryHistoryService.EditSalaryHistory(salaryHistory);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_EDIT_SALARY;
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