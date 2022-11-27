using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Security;
using KR.Web.Services;
using Kr.Models;
using Radzen;

namespace KR.Web.Pages.Users
{
    public partial class EditUser
    {


        [Inject]
        private UserService UserService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        private bool HaveErrors { get; set; }

        private string Error { get; set; }

        [Parameter]
        public dynamic Id_User { get; set; }

        private List<Role> rolesCheck { get; set; }

        private List<Role> roles { get; set; }

        private EditUserModel account = new EditUserModel();
        private User user = new User();
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        private async Task Load()
        {
            try
            {
                user = await UserService.GetUserById(Id_User);
                if (user == null)
                {
                    await Close(null);
                    return;
                }
                account.Last_name = user.Last_name;
                account.First_name = user.First_name;
                account.Otch = user.Otch;
                account.BirthDate = user.BirthDate;
                account.Email = user.Account.Email;
                account.Login = user.Account.Login;
                account.Roles = user.Account.Roles.ToList();
                if (account == null)
                {
                    await Close(null);
                    return;
                }
                rolesCheck = Role.GetRoles();
                roles = new List<Role>();
                foreach (Role role in rolesCheck)
                {
                    if (account.Roles.Find(x => x.Name == role.Name) != null)
                    {
                        roles.Add(account.Roles.Find(x => x.Name == role.Name));
                    }
                    else
                    {
                        roles.Add(role);
                    }
                }
            }
            catch
            {
                await Close(null);
            }
        }

        protected async Task Close(MouseEventArgs? args)
        {
            DialogService.Close(null);
        }


        private async Task HandleEdit()
        {
            try
            {
                User? obj = await UserService.GetUserByIdToReadOnly(Id_User);
                if (obj == null)
                {
                    Error = ConstantValues.ERROR_USER_EDIT;
                    HaveErrors = true;
                    return;
                }

                if (!UserService.CheckUserEmail(account.Email) && account.Email != obj.Account.Email)
                {
                    Error = ConstantValues.ERROR_REGISTER_EMAIL;
                    HaveErrors = true;
                    return;
                }

                if (!UserService.CheckUserLogin(account.Login) && account.Login != obj.Account.Login)
                {
                    Error = ConstantValues.ERROR_REGISTER_LOGIN;
                    HaveErrors = true;
                    return;
                }

                user.First_name = account.First_name;
                user.Last_name = account.Last_name;
                user.Otch = account.Otch;
                user.BirthDate = account.BirthDate;
                user.Account.Email = account.Email;
                user.Account.Login = account.Login;
                user.Account.Roles = account.Roles;
                if (!string.IsNullOrEmpty(account.Password))
                {
                    user.Account.Password = HashProvider.MakeHash(account.Password);
                }

                await UserService.EditUser(user);
                await Close(null);
            }
            catch (Exception ex)
            {
                Error = ConstantValues.ERROR_USER_EDIT;
                HaveErrors = true;
            }
        }
    }
}