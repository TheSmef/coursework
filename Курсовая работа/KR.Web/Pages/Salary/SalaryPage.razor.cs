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

namespace KR.Web.Pages.Salary
{
    public partial class SalaryPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<SalaryHistory>? grid;
        [Inject]
        private SalaryHistoryService SalaryHistoryService { get; set; }

        IEnumerable<SalaryHistory> getSalaryHistoryResult;
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