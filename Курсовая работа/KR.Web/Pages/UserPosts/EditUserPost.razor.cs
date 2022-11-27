using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.UserPosts
{
    public partial class EditUserPost
    {
        [Inject]
        private UserPostService UserPostService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        [Parameter]
        public dynamic Id_UserPost { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserPost userpostCheck = await UserPostService.GetUserPostById(Id_UserPost);
                if (userpostCheck == null)
                {
                    await Close(null);
                    return;
                }
                userpost = userpostCheck;
            }
            catch
            {
                await Close(null);
            }
        }


        private UserPost userpost = new UserPost();
        private async Task HandleEdit()
        {
            try
            {
                await UserPostService.EditUserPost(userpost);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_USERPOST_EDIT;
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