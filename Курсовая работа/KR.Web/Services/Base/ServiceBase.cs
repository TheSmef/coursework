using KR.API.Data;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services.Base
{
    public abstract class ServiceBase
    {
        protected readonly StoreDbContext storeDbContext;
        public ServiceBase(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
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