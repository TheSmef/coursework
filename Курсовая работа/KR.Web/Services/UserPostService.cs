using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using KR.Web.Services.Base;

namespace KR.Web.Services
{
    public class UserPostService : ServiceBase
    {

        public UserPostService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<UserPost>> GetUserPosts()
        {
            var items = storeDbContext.UserPosts.AsQueryable().Include(x => x.User).Include(x => x.Post);


            return await Task.FromResult(items);
        }

        public bool DeleteUserPost(UserPost userpost)
        {
            try
            {
                if (userpost != null)
                {
                    storeDbContext.UserPosts.Remove(userpost);
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

        public async Task EditUserPost(UserPost userpost)
        {
            if (userpost != null)
            {
                storeDbContext.UserPosts.Attach(userpost);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public async Task<UserPost?> GetUserPostById(Guid Id)
        {
            UserPost? user = await storeDbContext.UserPosts.Where(x => x.Id_User_Post == Id).Include(x => x.User).Include(x => x.Post).FirstOrDefaultAsync();
            return await Task.FromResult(user);
        }

        public async Task<UserPost?> GetUserPostByIdToReadOnly(Guid Id)
        {
            UserPost? user = await storeDbContext.UserPosts.Where(x => x.Id_User_Post == Id).Include(x => x.User).Include(x => x.Post).AsNoTracking().FirstOrDefaultAsync();
            return await Task.FromResult(user);
        }

        public bool CheckUserPost(UserPost userpost)
        {
            return !storeDbContext.UserPosts.Where(x => x.User == userpost.User && x.Post == userpost.Post).Any();
        }

        public async Task CreateUserPost(UserPost userPost)
        {
            if(userPost != null)
            {
                await storeDbContext.UserPosts.AddAsync(userPost);
                await storeDbContext.SaveChangesAsync();
            }
        }
    }
}
