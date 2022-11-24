using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using KR.Web.Services.Base;

namespace KR.Web.Services
{
    public class SalaryHistoryService : ServiceBase
    {
        public SalaryHistoryService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<SalaryHistory>> GetSalaryHistorys()
        {
            var items = storeDbContext.SalaryHistories.AsQueryable().Include(x => x.UserPost).ThenInclude(x => x.User).Include(x => x.UserPost).ThenInclude(x => x.Post);

            return await Task.FromResult(items);
        }

        public async Task<SalaryHistory?> GetSalaryHistoryById(Guid Id)
        {
            SalaryHistory? SalaryHistory = await storeDbContext.SalaryHistories.Where(x => x.Id_SalaryHistory == Id).Include(x => x.UserPost).ThenInclude(x => x.User).Include(x => x.UserPost).ThenInclude(x => x.Post).FirstOrDefaultAsync();
            return await Task.FromResult(SalaryHistory);
        }

        public async Task EditSalaryHistory(SalaryHistory SalaryHistory)
        {
            if (SalaryHistory != null)
            {
                storeDbContext.SalaryHistories.Attach(SalaryHistory);
                await storeDbContext.SaveChangesAsync();
            }
        }


        public async Task AddSalaryHistory(SalaryHistory SalaryHistory)
        {
            if (SalaryHistory != null)
            {
                await storeDbContext.SalaryHistories.AddAsync(SalaryHistory);
                await storeDbContext.SaveChangesAsync();
            }
        }


        public bool DeleteSalaryHistory(SalaryHistory SalaryHistory)
        {
            try
            {
                if (SalaryHistory != null)
                {
                    storeDbContext.SalaryHistories.Remove(SalaryHistory);
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
