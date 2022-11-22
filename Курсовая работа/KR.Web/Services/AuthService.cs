using Kr.Models;
using KR.API.Data;
using KR.Models;
using KR.Web.Models;
using KR.Web.Security;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class AuthService : ServiceBase
    {
        public AuthService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public Guid PrepareAuth(String login, String password)
        {
            Account? account = storeDbContext.Accounts.Where(x => x.Password == password).Where(x => x.Login == login).FirstOrDefault();
            if (account == null)
            {
                account = storeDbContext.Accounts.Where(x => x.Password == password).Where(x => x.Email == login).FirstOrDefault();
            }
            if (account == null)
            {
                return Guid.Empty;
            }
            Guid uid = account.UserId;
            return uid;
        }

        public Account? getAccountById(Guid UserId)
        {
            Account? account = storeDbContext.Accounts.Include(x => x.User).Include(x => x.Roles).Where(x => x.UserId == UserId).FirstOrDefault();
            return account;
        }

        public bool CheckEmailForUnique(String email)
        {
            return !storeDbContext.Accounts.Where(x => x.Email == email).Any();
        }

        public bool CheckLoginForUnique(String login)
        {
            return !storeDbContext.Accounts.Where(x => x.Login == login).Any();
        }

        public bool CreateNewAccount(RegistrationModel registrationData)
        {
            User user = new User();
            Account account = new Account();
            user.Last_name = registrationData.Last_name;
            user.First_name = registrationData.First_name;
            user.Otch = registrationData.Otch;
            user.BirthDate = registrationData.BirthDate;
            account.Email = registrationData.Email;
            account.Login = registrationData.Login;
            account.Password = HashProvider.MakeHash(registrationData.Password);
            account.Roles = registrationData.Roles;
            user.Account = account;
            

            storeDbContext.Users.Add(user);
            return (storeDbContext.SaveChanges() > 0);
        }

        public void Reload()
        {
            foreach (var entry in storeDbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
