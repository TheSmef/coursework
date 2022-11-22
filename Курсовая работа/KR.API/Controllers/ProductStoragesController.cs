using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET: api/ProductStorages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStorage>>> GetProductStorages()
        {
            return await _context.ProductStorages.Include(x => x.Category).ToListAsync();
        }

        // GET: api/ProductStorages/5
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

        // PUT: api/ProductStorages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/ProductStorages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductStorage>> PostProductStorage(ProductStorage productStorage)
        {
            _context.ProductStorages.Add(productStorage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductStorage", new { id = productStorage.Id_Product_Storage }, productStorage);
        }

        // DELETE: api/ProductStorages/5
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
