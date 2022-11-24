using KR.API.Data;
using Kr.Models;
using Microsoft.EntityFrameworkCore;
using Radzen;
using KR.Web.Services.Base;

namespace KR.Web.Services
{
    public class OrderProductService : ServiceBase
    {

        public OrderProductService(StoreDbContext storeDbContext) : base(storeDbContext)
        {

        }

        public async Task<IQueryable<OrderProduct>> GetOrderProducts()
        {
            var items = storeDbContext.OrderProducts.AsQueryable().Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.User)
                .Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.Post).Include(x => x.Product);
            return await Task.FromResult(items);
        }

        public async Task<OrderProduct?> GetOrderProductById(Guid Id)
        {
            OrderProduct? orderProduct = await storeDbContext.OrderProducts.Where(x => x.Id_order_product == Id).Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.User)
                .Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.Post).Include(x => x.Product).FirstOrDefaultAsync();
            return await Task.FromResult(orderProduct);
        }

        public bool CheckOrderProduct(OrderProduct orderproduct)
        {
            return !storeDbContext.OrderProducts.Where(x => x.Order == orderproduct.Order && x.Product == orderproduct.Product).Any();
        }

        public async Task EditOrderProduct(OrderProduct orderproduct)
        {
            if (orderproduct != null)
            {
                storeDbContext.OrderProducts.Attach(orderproduct);
                await storeDbContext.SaveChangesAsync();
                storeDbContext.Entry(orderproduct.Product).Reload();
            }
        }


        public async Task AddOrderProduct(OrderProduct orderproduct)
        {
            if (orderproduct != null)
            {
                await storeDbContext.OrderProducts.AddAsync(orderproduct);
                await storeDbContext.SaveChangesAsync();
                storeDbContext.Entry(orderproduct.Product).Reload();
            }
        }


        public bool DeleteOrderProduct(OrderProduct orderProduct)
        {
            try
            {
                if (orderProduct != null)
                {
                    storeDbContext.OrderProducts.Remove(orderProduct);
                    storeDbContext.SaveChanges();
                    storeDbContext.Entry(orderProduct.Product).Reload();
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
