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

namespace KR.Web.Pages.Salary
{
    public partial class AddSalary
    {
        [Inject]
        protected UserPostService UserPostService { get; set; }

        [Inject]
        protected SalaryHistoryService SalaryHistoryService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

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
            users = await UserPostService.GetUserPosts();
            grid?.Reload();
        }

        SalaryHistory salaryHistory = new SalaryHistory();
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