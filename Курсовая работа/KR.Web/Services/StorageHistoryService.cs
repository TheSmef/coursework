using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using KR.Web.Services.Base;

namespace KR.Web.Services
{
    public class StorageHistoryService : ServiceBase
    {

        public StorageHistoryService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

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
    }
}
