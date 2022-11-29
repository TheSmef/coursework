using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using KR.Web.Constants;
using KR.Web.Services;
using MudBlazor;

namespace KR.Web.Pages.Admin
{
    public partial class AdminPage
    {
        [Inject]
        private BackupService BackupService { get; set; }

        [Inject]
        private Radzen.DialogService DialogService { get; set; }


        private async Task GetDoc()
        {
            try
            {
                await BackupService.BackupDatabase();
            }
            catch (Exception e)
            {
                await DialogService.OpenAsync<ErrorBackUp>(ConstantValues.BACKUP_ERROR);
            }
        }

        private async void UploadFile(IBrowserFile file)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(file.Size).CopyToAsync(ms);
                byte[] data = ms.ToArray();
                await BackupService.RestoreBatabase(data);
                await DialogService.OpenAsync<SuccessRestoreDialog>(ConstantValues.RESTORE_TITLE);
            }
            catch (Exception e)
            {
                await DialogService.OpenAsync<ErrorRestore>(ConstantValues.RESTORE_ERROR);
            }
        }
    }
}