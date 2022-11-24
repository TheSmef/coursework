using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using KR.Web.Services.Base;

namespace KR.Web.Services
{
    public class PostService : ServiceBase
    {
        public PostService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<Post>> GetPosts()
        {
            var items = storeDbContext.Posts.AsQueryable();
            return await Task.FromResult(items);
        }
        public bool DeletePost(Post post)
        {
            try
            {
                if (post != null)
                {
                    storeDbContext.Posts.Remove(post);
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

        public async Task AddPost(Post post)
        {
            if (post != null)
            {
                await storeDbContext.Posts.AddAsync(post);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool CheckPostName(String name)
        {
            return !storeDbContext.Posts.Where(x => x.Name == name).Any();
        }

        public async Task<Post?> GetPostById(Guid Id)
        {
            Post? post = await storeDbContext.Posts.Where(x => x.Id_Post == Id).FirstOrDefaultAsync();
            return await Task.FromResult(post);
        }

        public async Task<Post?> GetPostByIdToReadOnly(Guid Id)
        {
            Post? post = await storeDbContext.Posts.Where(x => x.Id_Post == Id).AsNoTracking().FirstOrDefaultAsync();
            return await Task.FromResult(post);
        }

        public async Task EditPost(Post post)
        {
            if (post != null)
            {
                storeDbContext.Posts.Attach(post);
                await storeDbContext.SaveChangesAsync();
            }
        }

    }

}
