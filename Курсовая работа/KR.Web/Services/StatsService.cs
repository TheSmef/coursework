using Kr.Models;
using KR.API.Data;
using KR.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class StatsService : ServiceBase
    {

        public StatsService(StoreDbContext storeDbContext) : base(storeDbContext)
    {

        }

        public async Task<List<SalesStats>> GetStats(DateTime dateFrom, DateTime dateTo)
        {
            List<ProductStorage> items = storeDbContext.ProductStorages.AsQueryable().ToList();
            List<SalesStats> sales = new List<SalesStats>();
            foreach (ProductStorage item in items)
            {
                SalesStats stats = new SalesStats();
                stats.Name = item.Name;
                stats.SalesCount = 0;
                List<OrderProduct> orderProducts = storeDbContext.OrderProducts.AsQueryable().Include(x => x.Order).Where(x => x.Product == item && x.Order.Date_Order >= dateFrom && x.Order.Date_Order <= dateTo).ToList();
                try
                {
                    foreach (OrderProduct orderProduct in orderProducts)
                    {
                        stats.SalesCount += orderProduct.Amount;
                    }
                }
                catch
                {
                    stats.SalesCount = 0;
                }

                if (stats.SalesCount != 0)
                    sales.Add(stats);
            }
            return sales;
        }

        public async Task<List<SpentStats>> GetBuhStats(DateTime dateFrom, DateTime dateTo)
        {
            List<SalaryHistory> salary = storeDbContext.SalaryHistories.AsQueryable().Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToList();
            List<Order> orders = storeDbContext.Orders.AsQueryable().Where(x => x.Date_Order >= dateFrom && x.Date_Order <= dateTo).Include(x => x.OrderProducts).ToList();
            List<PurchaseAgreement> purchases = storeDbContext.PurchaseAgreements.AsQueryable().Where(x => x.Date_Of_Purchase >= dateFrom && x.Date_Of_Purchase <= dateTo).Include(x => x.Purchases).ToList();
            List<SpentStats> stats = new List<SpentStats>();
            SpentStats statsalary = new SpentStats();
            statsalary.Name = "Зарплаты";
            statsalary.Summ = 0;
            try
            {
                foreach (SalaryHistory salaryHistory in salary)
                {
                    statsalary.Summ -= (double)salaryHistory.Payment;
                }
            }
            catch
            {
                statsalary.Summ = 0;
            }

            SpentStats statorder = new SpentStats();
            statorder.Name = "Заказы";
            statorder.Summ = 0;
            try
            {
                foreach (Order order in orders)
                {
                    foreach (OrderProduct orderProduct in order.OrderProducts)
                    {
                        statorder.Summ += (double)orderProduct.Price;
                    }
                }
            }
            catch
            {

            }
            SpentStats statpurchase = new SpentStats();
            statpurchase.Name = "Закупки";
            statpurchase.Summ = 0;
            try
            {
                foreach (PurchaseAgreement purchase in purchases)
                {
                    foreach (Purchase purchaseAct in purchase.Purchases)
                    {
                        statpurchase.Summ -= (double)purchaseAct.Price;
                    }
                }
            }
            catch
            {

            }
            stats.Add(statpurchase);
            stats.Add(statorder);
            stats.Add(statsalary);
            return stats;
        }
    }
}
