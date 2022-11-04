using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using KR.Models;
using KR.Web.Models;
using KR.Web.Security;

namespace KR.Web.Services
{
    public class UserService
    {
        private readonly StoreDbContext storeDbContext;
        public UserService(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
        }

        public async Task<IQueryable<User>> GetUsers()
        {
            var items = storeDbContext.Users.AsQueryable().Include(x => x.Account).ThenInclude(x => x.Roles);
            return await Task.FromResult(items);
        }

        public async Task<User?> GetUserById(Guid Id)
        {
            User? user = await storeDbContext.Users.Where(x => x.Id_User == Id).Include(x => x.Account).ThenInclude(x => x.Roles).FirstOrDefaultAsync();
            return await Task.FromResult(user);
        }

        public async Task<User?> GetUserByIdToReadOnly(Guid Id)
        {
            User? user = await storeDbContext.Users.Where(x => x.Id_User == Id).AsNoTracking().Include(x => x.Account).ThenInclude(x => x.Roles).FirstOrDefaultAsync();
            return await Task.FromResult(user);
        }

        public bool CheckUserLogin(String login)
        {
            return !storeDbContext.Accounts.Where(x => x.Login == login).Any();
        }

        public bool CheckUserEmail(String email)
        {
            return !storeDbContext.Accounts.Where(x => x.Email == email).Any();
        }

        public async Task EditUser(User user)
        {
            if (user != null)
            {
                storeDbContext.Users.Attach(user);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool DeleteUser(User user)
        {
            try
            {
                if (user != null)
                {
                    storeDbContext.Users.Remove(user);
                    storeDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

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
