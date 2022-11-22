using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
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