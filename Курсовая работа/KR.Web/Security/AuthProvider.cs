using Blazored.LocalStorage;
using Kr.Models;
using KR.Models;
using KR.Web.Constants;
using KR.Web.Models;
using KR.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KR.Web.Security
{
    public class AuthProvider : AuthenticationStateProvider
    {
        public readonly ILocalStorageService LocalStorage;
        public readonly AuthService authService;
        public AuthProvider(ILocalStorageService localStorage, AuthService authService)
        {
            LocalStorage = localStorage;
            this.authService = authService;
        }



        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            UserData user = await LocalStorage.GetItemAsync<UserData>(ConstantValues.USER_LOCATION);

            if (user == null)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }

            Account? account = authService.getAccountById(user.UserId);

            if (account == null)
            {
                await LocalStorage.RemoveItemAsync(ConstantValues.USER_LOCATION);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }

            if ((account.Login == user.Login || account.Email == user.Login) && account.Password == user.Password)
            {
                var identity = new ClaimsIdentity(ConstantValues.USER_AUTH_TYPE);
                if (account.Roles != null)
                {
                    foreach (Role role in account.Roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                await LocalStorage.RemoveItemAsync(ConstantValues.USER_LOCATION);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

    }
}
