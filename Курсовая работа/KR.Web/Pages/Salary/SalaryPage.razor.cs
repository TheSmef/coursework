using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
using KR.Web.Services;

namespace KR.Web.Pages.Salary
{
    public partial class SalaryPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<SalaryHistory>? grid;
        [Inject]
        private SalaryHistoryService SalaryHistoryService { get; set; }

        private IEnumerable<SalaryHistory> getSalaryHistoryResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getSalaryHistoryResult = await SalaryHistoryService.GetSalaryHistorys();
        }

        private async Task AddSalaryHistory()
        {
            await DialogService.OpenAsync<AddSalary>(ConstantValues.ADD_SALARY);
            SalaryHistoryService.Reload();
            await grid.Reload();
        }

        private async Task DeleteSalaryHistory(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!SalaryHistoryService.DeleteSalaryHistory(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    SalaryHistoryService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task SalaryHistoryRowSelect(SalaryHistory args)
        {
            await DialogService.OpenAsync<EditSalary>(ConstantValues.EDIT_SALARY, new Dictionary<string, object>()
            {{ConstantValues.Id_SalaryHistory, args.Id_SalaryHistory}});
            SalaryHistoryService.Reload();
            await grid.Reload();
        }
    }
}