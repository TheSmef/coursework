using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

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
            try
            {
                Post postCheck = await PostService.GetPostById(Id_Post);
                if (postCheck == null)
                {
                    await Close(null);
                    return;
                }

                post = postCheck;
            }
            catch
            {
                await Close(null);
            }
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