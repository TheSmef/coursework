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
using BlazorDownloadFile;
using Blazored.LocalStorage;
using KR.Web.Constants;
using Kr.Models;
using System.ComponentModel;
using KR.Web.Services;
using MudBlazor;

namespace KR.Web.Pages
{
    public partial class AdminPage
    {
        [Inject]
        private BackupService BackupService { get; set; }

        [Inject]
        private Radzen.DialogService DialogService { get; set; }

        [Inject]
        private IBlazorDownloadFileService blazorDownloadFileService { get; set; }

        private async Task GetDoc()
        {
            try
            {
                await BackupService.BackupDatabase();
            }
            catch
            {
                await DialogService.OpenAsync<ErrorBackUp>(ConstantValues.BACKUP_ERROR);
            }
        }

        private async void UploadFile(IBrowserFile file)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(5120000000).CopyToAsync(ms);
                byte[] data = ms.ToArray();
                await BackupService.RestoreBatabase(data);
                await DialogService.OpenAsync<SuccessRestoreDialog>(ConstantValues.RESTORE_TITLE);
            }
            catch
            {
                await DialogService.OpenAsync<ErrorRestore>(ConstantValues.RESTORE_ERROR);
            }
        }
    }
}