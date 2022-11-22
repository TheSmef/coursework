using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class OrderService : ServiceBase
    {
        public OrderService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<Order>> GetOrders()
        {
            var items = storeDbContext.Orders.AsQueryable().Include(x => x.UserPost).ThenInclude(x => x.User).Include(x => x.UserPost).ThenInclude(x => x.Post);

            return await Task.FromResult(items);
        }

        public async Task<Order?> GetOrderById(Guid Id)
        {
            Order? order = await storeDbContext.Orders.Where(x => x.Id_Order == Id).Include(x => x.UserPost).ThenInclude(x => x.User).Include(x => x.UserPost).ThenInclude(x => x.Post).FirstOrDefaultAsync();
            return await Task.FromResult(order);
        }

        public async Task EditOrder(Order order)
        {
            if (order != null)
            {
                storeDbContext.Orders.Attach(order);
                await storeDbContext.SaveChangesAsync();
            }
        }


        public async Task AddOrder(Order order)
        {
            if (order != null)
            {
                await storeDbContext.Orders.AddAsync(order);
                order.Order_Number = order.Id_Order.ToString();
                await storeDbContext.SaveChangesAsync();
            }
        }

        public bool DeleteOrder(Order order)
        {
            try
            {
                if (order != null)
                {
                    storeDbContext.Orders.Remove(order);
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
