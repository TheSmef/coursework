using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KR.API.Data;
using Kr.Models;

namespace KR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public PurchasesController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            return await _context.Purchases.Include(x => x.ProductStorage).ThenInclude(x => x.Category).Include(x => x.PurchaseAgreement).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }
            _context.Entry(purchase).Reference(x => x.ProductStorage).Load();
            _context.Entry(purchase.ProductStorage).Reference(x => x.Category).Load();
            _context.Entry(purchase).Reference(x => x.PurchaseAgreement).Load();
            return purchase;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(Guid id, Purchase purchase)
        {
            if (id != purchase.Id_Puchase)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchase", new { id = purchase.Id_Puchase }, purchase);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(Guid id)
        {
            return _context.Purchases.Any(e => e.Id_Puchase == id);
        }
    }
}
