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

namespace KR.Web.Pages.UserPosts
{
    public partial class AddUserPost
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private UserService UserService { get; set; }

        [Inject]
        private PostService PostService { get; set; }

        [Inject]
        private UserPostService UserPostService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private RadzenDropDownDataGrid<User>? grid;
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private IEnumerable<User> users { get; set; }

        private IEnumerable<Post> posts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            users = (await UserService.GetUsers()).ToList();
            posts = (await PostService.GetPosts()).ToList();
            grid?.Reload();
        }

        private UserPost userpost = new UserPost();
        private async Task HandleUserPostCreation()
        {
            try
            {
                if (UserPostService.CheckUserPost(userpost))
                {
                    await UserPostService.CreateUserPost(userpost);
                    await Close(null);
                }
                else
                {
                    Error = ConstantValues.ERROR_USERPOST_EXISTS;
                    HaveErrors = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_USERPOST_ADD;
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