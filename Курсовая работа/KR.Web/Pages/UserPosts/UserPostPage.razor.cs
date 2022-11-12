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

namespace KR.Web.Pages.UserPosts
{
    public partial class UserPostPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<UserPost>? grid;
        [Inject]
        private UserPostService UserPostService { get; set; }

        IEnumerable<UserPost> getUserPostResult;
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