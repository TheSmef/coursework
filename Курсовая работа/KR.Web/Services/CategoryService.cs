using Kr.Models;
using KR.API.Data;
using KR.Web.Services.Base;
using Microsoft.EntityFrameworkCore;



namespace KR.Web.Services
{
    public class CategoryService : ServiceBase
    {

        public CategoryService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<Category>> GetCategories()
        {
            var items = storeDbContext.Categories.AsQueryable();
            
            return await Task.FromResult(items);
        }

        public async Task<Category?> GetCategoryById(Guid Id)
        {
            Category? category = await storeDbContext.Categories.Where(x => x.Id_Category == Id).FirstOrDefaultAsync();
            return await Task.FromResult(category);
        }
        public async Task<Category?> GetCategoryByIdForReadOnly(Guid Id)
        {
            Category? category = await storeDbContext.Categories.Where(x => x.Id_Category == Id).AsNoTracking().FirstOrDefaultAsync();
            return await Task.FromResult(category);
        }

        public async Task EditCategory(Category category)
        {
            if (category != null)
            {
                storeDbContext.Categories.Attach(category);
                await storeDbContext.SaveChangesAsync();
            }
        }


        public async Task AddCategory(Category category)
        {
            if (category != null)
            {
                await storeDbContext.Categories.AddAsync(category);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool CheckCategoryName(String name)
        {
            return !storeDbContext.Categories.Where(x => x.Name == name).Any();
        }


        public bool DeleteCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    storeDbContext.Categories.Remove(category);
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
    }
}
