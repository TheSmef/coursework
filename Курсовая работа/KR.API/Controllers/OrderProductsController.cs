using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KR.API.Data;
using Kr.Models;

namespace KR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductsController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public OrderProductsController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderProduct>>> GetOrderProducts()
        {
            return await _context.OrderProducts.Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.User)
                .Include(x => x.Order).ThenInclude(x => x.UserPost).ThenInclude(x => x.Post).Include(x => x.Product).ThenInclude(x => x.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProduct>> GetOrderProduct(Guid id)
        {
            var orderProduct = await _context.OrderProducts.FindAsync(id);

            if (orderProduct == null)
            {
                return NotFound();
            }
            _context.Entry(orderProduct).Reference(x => x.Order).Load();
            _context.Entry(orderProduct.Order).Reference(x => x.UserPost).Load();
            _context.Entry(orderProduct.Order.UserPost).Reference(x => x.User).Load();
            _context.Entry(orderProduct.Order.UserPost).Reference(x => x.Post).Load();
            _context.Entry(orderProduct).Reference(x => x.Product).Load();
            _context.Entry(orderProduct.Product).Reference(x => x.Category).Load();
            return orderProduct;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderProduct(Guid id, OrderProduct orderProduct)
        {
            if (id != orderProduct.Id_order_product)
            {
                return BadRequest();
            }

            _context.Entry(orderProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<OrderProduct>> PostOrderProduct(OrderProduct orderProduct)
        {
            _context.OrderProducts.Add(orderProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderProduct", new { id = orderProduct.Id_order_product }, orderProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProduct(Guid id)
        {
            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            _context.OrderProducts.Remove(orderProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderProductExists(Guid id)
        {
            return _context.OrderProducts.Any(e => e.Id_order_product == id);
        }
    }
}
