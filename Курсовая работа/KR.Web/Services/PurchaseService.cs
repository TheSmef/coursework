using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class PurchaseService : ServiceBase
    {

        public PurchaseService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<Purchase>> GetPurchases()
        {
            var items = storeDbContext.Purchases.AsQueryable().Include(x => x.PurchaseAgreement)
                .Include(x => x.ProductStorage).ThenInclude(x => x.Category);
            return await Task.FromResult(items);
        }

        public async Task<Purchase?> GetPurchaseById(Guid Id)
        {
            Purchase? orderProduct = await storeDbContext.Purchases.Where(x => x.Id_Puchase == Id).Include(x => x.PurchaseAgreement)
                .Include(x => x.ProductStorage).ThenInclude(x => x.Category).FirstOrDefaultAsync();
            return await Task.FromResult(orderProduct);
        }

        public bool CheckPurchase(Purchase purchase)
        {
            return !storeDbContext.Purchases.Where(x => x.PurchaseAgreement == purchase.PurchaseAgreement && x.ProductStorage == purchase.ProductStorage).Any();
        }

        public async Task EditPurchase(Purchase purchase)
        {
            if (purchase != null)
            {
                storeDbContext.Purchases.Attach(purchase);
                await storeDbContext.SaveChangesAsync();
                storeDbContext.Entry(purchase.ProductStorage).Reload();
            }
        }


        public async Task AddPurchase(Purchase purchase)
        {
            if (purchase != null)
            {
                await storeDbContext.Purchases.AddAsync(purchase);
                await storeDbContext.SaveChangesAsync();
                storeDbContext.Entry(purchase.ProductStorage).Reload();
            }
        }


        public bool DeletePurchase(Purchase purchase)
        {
            try
            {
                if (purchase != null)
                {
                    storeDbContext.Purchases.Remove(purchase);
                    storeDbContext.SaveChanges();
                    storeDbContext.Entry(purchase.ProductStorage).Reload();
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
