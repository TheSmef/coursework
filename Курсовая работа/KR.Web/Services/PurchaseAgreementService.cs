using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class PurchaseAgreementService : ServiceBase
    {

        public PurchaseAgreementService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<PurchaseAgreement>> GetPurchaseAgreements()
        {
            var items = storeDbContext.PurchaseAgreements.AsQueryable();

            return await Task.FromResult(items);
        }

        public async Task<PurchaseAgreement?> GetPurchaseAgreementById(Guid Id)
        {
            PurchaseAgreement? purchaseAgreement = await storeDbContext.PurchaseAgreements.Where(x => x.Id_Purchase_Agreement == Id).FirstOrDefaultAsync();
            return await Task.FromResult(purchaseAgreement);
        }

        public async Task EditPurchaseAgreement(PurchaseAgreement purchaseAgreement)
        {
            if (purchaseAgreement != null)
            {
                storeDbContext.PurchaseAgreements.Attach(purchaseAgreement);
                await storeDbContext.SaveChangesAsync();
            }
        }


        public async Task AddPurchaseAgreement(PurchaseAgreement purchaseAgreement)
        {
            if (purchaseAgreement != null)
            {
                await storeDbContext.PurchaseAgreements.AddAsync(purchaseAgreement);
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool DeletePurchaseAgreement(PurchaseAgreement purchaseAgreement)
        {
            try
            {
                if (purchaseAgreement != null)
                {
                    storeDbContext.PurchaseAgreements.Remove(purchaseAgreement);
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
