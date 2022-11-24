using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
using KR.Web.Services;
using BlazorDownloadFile;

namespace KR.Web.Pages.Users
{
    public partial class UserPage
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<User>? grid;
        [Inject]
        private UserService UserService { get; set; }
        [Inject]
        private ExportService ExportService { get; set; }
        [Inject]
        private IBlazorDownloadFileService blazorDownloadFileService { get; set; }

        private IEnumerable<User> getUserResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getUserResult = await UserService.GetUsers();
        }

        private async Task AddUser()
        {
            await DialogService.OpenAsync<AddUser>(ConstantValues.ADD_USER);
            UserService.Reload();
            await grid.Reload();
        }

        private async Task GetDoc()
        {
            try
            {
                await ExportService.ExportUsersToCsv(grid.View.ToList());
            }
            catch
            {
                await DialogService.OpenAsync<ErrorExport>(ConstantValues.EXPORT_ERROR);
            }

        }


        private async Task DeleteUser(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!UserService.DeleteUser(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }
                    UserService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task UserRowSelect(User args)
        {
            await DialogService.OpenAsync<EditUser>(ConstantValues.EDIT_USER, new Dictionary<string, object>()
            {{ConstantValues.Id_User, args.Id_User}});
            UserService.Reload();
            await grid.Reload();
        }


        private async void UploadFile(IBrowserFile file)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(file.Size).CopyToAsync(ms);
                byte[] data = ms.ToArray();
                ExportService.ImportUsersFromCvs(data);
                grid.Reload();
            }
            catch
            {
                await DialogService.OpenAsync<ErrorImport>(ConstantValues.IMPORT_ERROR);
            }
        }

    }
}