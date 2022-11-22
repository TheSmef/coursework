using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KR.API.Data;
using Kr.Models;

namespace KR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoragesController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public ProductStoragesController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStorage>>> GetProductStorages()
        {
            return await _context.ProductStorages.Include(x => x.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStorage>> GetProductStorage(Guid id)
        {
            var productStorage = await _context.ProductStorages.FindAsync(id);
            

            if (productStorage == null)
            {
                return NotFound();
            }
            _context.Entry(productStorage).Reference(x => x.Category).Load();
            return productStorage;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductStorage(Guid id, ProductStorage productStorage)
        {
            if (id != productStorage.Id_Product_Storage)
            {
                return BadRequest();
            }

            _context.Entry(productStorage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductStorageExists(id))
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
        public async Task<ActionResult<ProductStorage>> PostProductStorage(ProductStorage productStorage)
        {
            _context.ProductStorages.Add(productStorage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStorage", new { id = productStorage.Id_Product_Storage }, productStorage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductStorage(Guid id)
        {
            var productStorage = await _context.ProductStorages.FindAsync(id);
            if (productStorage == null)
            {
                return NotFound();
            }

            _context.ProductStorages.Remove(productStorage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductStorageExists(Guid id)
        {
            return _context.ProductStorages.Any(e => e.Id_Product_Storage == id);
        }
    }
}
