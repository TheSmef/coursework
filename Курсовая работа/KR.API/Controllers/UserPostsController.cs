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
    public class UserPostsController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public UserPostsController(StoreDbContext context)
        {
            _context = context;
        }

        // GET: api/UserPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPost>>> GetUserPosts()
        {
            return await _context.UserPosts.Include(x => x.User).Include(x => x.Post).ToListAsync();
        }

        // GET: api/UserPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPost>> GetUserPost(Guid id)
        {
            var userPost = await _context.UserPosts.FindAsync(id);

            if (userPost == null)
            {
                return NotFound();
            }
            _context.Entry(userPost).Reference(x => x.User).Load();
            _context.Entry(userPost).Reference(x => x.Post).Load();
            return userPost;
        }

        // PUT: api/UserPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPost(Guid id, UserPost userPost)
        {
            if (id != userPost.Id_User_Post)
            {
                return BadRequest();
            }

            _context.Entry(userPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPostExists(id))
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

        // POST: api/UserPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPost>> PostUserPost(UserPost userPost)
        {
            _context.UserPosts.Add(userPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPost", new { id = userPost.Id_User_Post }, userPost);
        }

        // DELETE: api/UserPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPost(Guid id)
        {
            var userPost = await _context.UserPosts.FindAsync(id);
            if (userPost == null)
            {
                return NotFound();
            }

            _context.UserPosts.Remove(userPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPostExists(Guid id)
        {
            return _context.UserPosts.Any(e => e.Id_User_Post == id);
        }
    }
}
