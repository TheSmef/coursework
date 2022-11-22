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

namespace KR.Web.Pages.StorageHistories
{
    public partial class StorageHistoryPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<StorageHistory>? grid;
        [Inject]
        private StorageHistoryService StorageHistoryService { get; set; }

        private IEnumerable<StorageHistory> getStorageHistoryResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getStorageHistoryResult = await StorageHistoryService.GetStorageHistory();
        }

        private async Task DeleteStorageHistory(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!StorageHistoryService.DeleteStorageHistory(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    StorageHistoryService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }
    }
}