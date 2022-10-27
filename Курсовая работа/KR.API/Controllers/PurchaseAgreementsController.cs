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
    public class PurchaseAgreementsController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public PurchaseAgreementsController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseAgreements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseAgreement>>> GetPurchaseAgreements()
        {
            return await _context.PurchaseAgreements.Include(x => x.Purchases).ToListAsync();
        }

        // GET: api/PurchaseAgreements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseAgreement>> GetPurchaseAgreement(Guid id)
        {
            var purchaseAgreement = await _context.PurchaseAgreements.FindAsync(id);

            if (purchaseAgreement == null)
            {
                return NotFound();
            }
            _context.Entry(purchaseAgreement).Reference(x => x.Purchases).Load();
            return purchaseAgreement;
        }

        // PUT: api/PurchaseAgreements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseAgreement(Guid id, PurchaseAgreement purchaseAgreement)
        {
            if (id != purchaseAgreement.Id_Purchase_Agreement)
            {
                return BadRequest();
            }

            _context.Entry(purchaseAgreement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseAgreementExists(id))
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

        // POST: api/PurchaseAgreements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchaseAgreement>> PostPurchaseAgreement(PurchaseAgreement purchaseAgreement)
        {
            _context.PurchaseAgreements.Add(purchaseAgreement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseAgreement", new { id = purchaseAgreement.Id_Purchase_Agreement }, purchaseAgreement);
        }

        // DELETE: api/PurchaseAgreements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseAgreement(Guid id)
        {
            var purchaseAgreement = await _context.PurchaseAgreements.FindAsync(id);
            if (purchaseAgreement == null)
            {
                return NotFound();
            }

            _context.PurchaseAgreements.Remove(purchaseAgreement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseAgreementExists(Guid id)
        {
            return _context.PurchaseAgreements.Any(e => e.Id_Purchase_Agreement == id);
        }
    }
}
