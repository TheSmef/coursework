using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KR.Web.Services
{
    public class ProductService
    {
        private readonly StoreDbContext storeDbContext;
        public ProductService(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
        }

        public async Task<IQueryable<ProductStorage>> GetProducts()
        {
            var items = storeDbContext.ProductStorages.AsQueryable().Include(x => x.Category);

            return await Task.FromResult(items);
        }

        public async Task<ProductStorage?> GetProductByIdToReadOnly(Guid Id)
        {
            ProductStorage? productStorage = await storeDbContext.ProductStorages.Where(x => x.Id_Product_Storage == Id).Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync();
            return await Task.FromResult(productStorage);
        }

        public async Task<ProductStorage?> GetProductById(Guid Id)
        {
            ProductStorage? productStorage = await storeDbContext.ProductStorages.Where(x => x.Id_Product_Storage == Id).Include(x => x.Category).FirstOrDefaultAsync();
            return await Task.FromResult(productStorage);
        }

        public bool DeleteProduct(ProductStorage productStorage)
        {
            try
            {
                if (productStorage != null)
                {
                    storeDbContext.ProductStorages.Remove(productStorage);
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

        public async Task EditProduct(ProductStorage productStorage)
        {
            if (productStorage != null)
            {
                storeDbContext.ProductStorages.Attach(productStorage);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public async Task AddProduct(ProductStorage productStorage)
        {
            if (productStorage != null)
            {
                await storeDbContext.ProductStorages.AddAsync(productStorage);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool CheckProductName(String name)
        {
            return !storeDbContext.ProductStorages.Where(x => x.Name == name).Any();
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
