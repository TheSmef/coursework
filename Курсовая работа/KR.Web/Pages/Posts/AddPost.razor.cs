using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Posts
{
    public partial class AddPost
    {
        [Inject]
        private PostService PostService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private Post? post = new Post();
        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        private async Task HandleAdd()
        {
            try
            {
                if (!PostService.CheckPostName(post.Name))
                {
                    Error = ConstantValues.ERROR_POST_NAME_EXISTS;
                    HaveErrors = true;
                    return;
                }
                await PostService.AddPost(post);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_POST_ADD;
                HaveErrors = true;
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }
    }
}