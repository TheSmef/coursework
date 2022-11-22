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

namespace KR.Web.Pages.Posts
{
    public partial class EditPost
    {
        [Inject]
        private PostService PostService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private Post? post = new Post();
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        [Parameter]
        public dynamic Id_Post { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Post postCheck = await PostService.GetPostById(Id_Post);
            if (postCheck == null)
            {
                await Close(null);
                return;
            }

            post = postCheck;
        }

        private async Task HandleEdit()
        {
            try
            {
                Post? obj = await PostService.GetPostByIdToReadOnly(post.Id_Post);
                if (obj == null)
                {
                    Error = ConstantValues.ERROR_POST_EDIT;
                    HaveErrors = true;
                    return;
                }

                if (!PostService.CheckPostName(post.Name) && post.Name != obj.Name)
                {
                    Error = ConstantValues.ERROR_POST_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }

                await PostService.EditPost(post);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_POST_EDIT;
                HaveErrors = true;
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }
    }
}