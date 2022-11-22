using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using KR.Web.Constants;
using Kr.Models;
using KR.Web.Services;

namespace KR.Web.Pages.UserPosts
{
    public partial class UserPostPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<UserPost>? grid;
        [Inject]
        private UserPostService UserPostService { get; set; }

        private IEnumerable<UserPost> getUserPostResult;
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getUserPostResult = await UserPostService.GetUserPosts();
        }

        private async Task AddUserPost()
        {
            await DialogService.OpenAsync<AddUserPost>(ConstantValues.ADD_USERPOST);
            UserPostService.Reload();
            await grid.Reload();
        }

        private async Task DeleteUserPost(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!UserPostService.DeleteUserPost(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }

                    UserPostService.Reload();
                    await grid.Reload();
                }
            }
            catch
            {
                await grid.Reload();
            }
        }

        private async Task UserPostRowSelect(UserPost args)
        {
            await DialogService.OpenAsync<EditUserPost>(ConstantValues.EDIT_USERPOST, new Dictionary<string, object>()
            {{ConstantValues.Id_UserPost, args.Id_User_Post}});
            UserPostService.Reload();
            await grid.Reload();
        }
    }
}