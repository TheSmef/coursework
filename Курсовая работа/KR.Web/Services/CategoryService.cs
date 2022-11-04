using Kr.Models;
using KR.API.Data;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace KR.Web.Services
{
    public class CategoryService
    {
        private readonly StoreDbContext storeDbContext;
        public CategoryService(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
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
