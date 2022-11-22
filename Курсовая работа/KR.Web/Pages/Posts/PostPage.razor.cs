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
using KR.Web.Pages.Categories;

namespace KR.Web.Pages.Posts
{
    public partial class PostPage
    {
        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDataGrid<Post>? grid;

        [Inject]
        private PostService PostService { get; set; }

        private IEnumerable<Post> getPostResult;

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            getPostResult = await PostService.GetPosts();
        }

        private async Task PostRowSelect(Post args)
        {
            await DialogService.OpenAsync<EditPost>(ConstantValues.EDIT_POST, new Dictionary<string, object>() { { ConstantValues.Id_Post, args.Id_Post } });
            await grid.Reload();
        }

        private async Task AddPost()
        {
            await DialogService.OpenAsync<AddPost>(ConstantValues.ADD_USER);
            PostService.Reload();
            await grid.Reload();
        }




        private async Task DeletePost(MouseEventArgs args, dynamic data)
        {
            try
            {
                ConfirmOptions options = new ConfirmOptions();
                options.CancelButtonText = ConstantValues.CANCEL;
                options.OkButtonText = ConstantValues.OK_DELETE;
                if (await DialogService.Confirm(ConstantValues.DELETE_RECORD, ConstantValues.DELETE_RECORD_TITLE, options) == true)
                {
                    if (!PostService.DeletePost(data))
                    {
                        await DialogService.OpenAsync<ErrorDelete>(ConstantValues.DELETE_ERROR);
                    }
                    PostService.Reload();
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