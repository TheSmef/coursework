using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KR.API.Data;
using Kr.Models;

namespace KR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryHistoriesController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public SalaryHistoriesController(StoreDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryHistory>>> GetSalaryHistories()
        {
            return await _context.SalaryHistories.Include(x => x.UserPost).ThenInclude(x => x.User).Include(x => x.UserPost).ThenInclude(x => x.Post).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryHistory>> GetSalaryHistory(Guid id)
        {
            var salaryHistory = await _context.SalaryHistories.FindAsync(id);

            if (salaryHistory == null)
            {
                return NotFound();
            }
            _context.Entry(salaryHistory).Reference(x => x.UserPost).Load();
            _context.Entry(salaryHistory.UserPost).Reference(x => x.User).Load();
            _context.Entry(salaryHistory.UserPost).Reference(x => x.Post).Load();
            return salaryHistory;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalaryHistory(Guid id, SalaryHistory salaryHistory)
        {
            if (id != salaryHistory.Id_SalaryHistory)
            {
                return BadRequest();
            }

            _context.Entry(salaryHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryHistoryExists(id))
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
        public async Task<ActionResult<SalaryHistory>> PostSalaryHistory(SalaryHistory salaryHistory)
        {
            _context.SalaryHistories.Add(salaryHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalaryHistory", new { id = salaryHistory.Id_SalaryHistory }, salaryHistory);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalaryHistory(Guid id)
        {
            var salaryHistory = await _context.SalaryHistories.FindAsync(id);
            if (salaryHistory == null)
            {
                return NotFound();
            }

            _context.SalaryHistories.Remove(salaryHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaryHistoryExists(Guid id)
        {
            return _context.SalaryHistories.Any(e => e.Id_SalaryHistory == id);
        }
    }
}
