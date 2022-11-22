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
    public class StorageHistoriesController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public StorageHistoriesController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: api/StorageHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StorageHistory>>> GetStorageHistory()
        {
            return await _context.StorageHistory.Include(x => x.ProductStorage).ThenInclude(x => x.Category)
                .ToListAsync();
        }

        // GET: api/StorageHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StorageHistory>> GetStorageHistory(Guid id)
        {
            var storageHistory = await _context.StorageHistory.FindAsync(id);

            if (storageHistory == null)
            {
                return NotFound();
            }
            _context.Entry(storageHistory).Reference(x => x.ProductStorage).Load();
            _context.Entry(storageHistory.ProductStorage).Reference(x => x.Category).Load();
            return storageHistory;
        }

        // DELETE: api/StorageHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorageHistory(Guid id)
        {
            var storageHistory = await _context.StorageHistory.FindAsync(id);
            if (storageHistory == null)
            {
                return NotFound();
            }

            _context.StorageHistory.Remove(storageHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StorageHistoryExists(Guid id)
        {
            return _context.StorageHistory.Any(e => e.Id_StorageHistory == id);
        }
    }
}
