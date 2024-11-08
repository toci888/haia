using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly ComedyDbContext _context;

        public FriendshipController(ComedyDbContext context)
        {
            _context = context;
        }

        // GET: api/Friendship/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FriendshipDto>>> GetFriendshipsByUser(int userId)
        {
            var friendships = await _context.Friendships
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Include(f => f.User)
                .Include(f => f.Friend)
                .Select(f => new FriendshipDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId,
                    UserName = f.User.Username, // assuming User has a Name property
                    FriendName = f.Friend.Username// assuming Friend has a Name property
                })
                .ToListAsync();

            return Ok(friendships);
        }

        // POST: api/Friendship
        [HttpPost]
        public async Task<ActionResult<FriendshipDto>> CreateFriendship(CreateFriendshipDto createFriendshipDto)
        {
            if (createFriendshipDto.UserId == createFriendshipDto.FriendId)
            {
                return BadRequest("User cannot be friends with themselves.");
            }

            // Check if friendship already exists
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.UserId == createFriendshipDto.UserId && f.FriendId == createFriendshipDto.FriendId) ||
                    (f.UserId == createFriendshipDto.FriendId && f.FriendId == createFriendshipDto.UserId));

            if (existingFriendship != null)
            {
                return Conflict("Friendship already exists.");
            }

            var friendship = new Friendship
            {
                UserId = createFriendshipDto.UserId,
                FriendId = createFriendshipDto.FriendId
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            var friendshipDto = new FriendshipDto
            {
                Id = friendship.Id,
                UserId = friendship.UserId,
                FriendId = friendship.FriendId,
                UserName = (await _context.Users.FindAsync(friendship.UserId))?.Username,
                FriendName = (await _context.Users.FindAsync(friendship.FriendId))?.Username
            };

            return CreatedAtAction(nameof(GetFriendshipsByUser), new { userId = friendship.UserId }, friendshipDto);
        }

        // DELETE: api/Friendship/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendship(int id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
