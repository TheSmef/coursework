using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class StorageHistoryService
    {
        private readonly StoreDbContext storeDbContext;
        public StorageHistoryService(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
        }

        public async Task<IQueryable<StorageHistory>> GetStorageHistory()
        {
            var items = storeDbContext.StorageHistory.AsQueryable().Include(x => x.ProductStorage);
            return await Task.FromResult(items);
        }

        public bool DeleteStorageHistory(StorageHistory storageHistory)
        {
            try
            {
                if (storageHistory != null)
                {
                    storeDbContext.StorageHistory.Remove(storageHistory);
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
