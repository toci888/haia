using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;
using Microsoft.AspNetCore.SignalR;

namespace Toci.Haia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly ComedyDbContext _context;
        private readonly IHubContext<FriendRequestHub> _hubContext;

        public FriendshipController(ComedyDbContext context, IHubContext<FriendRequestHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: api/Friendship/invitations/{userId}
        [HttpGet("invitations/{userId}")]
        public async Task<ActionResult<IEnumerable<FriendshipDto>>> GetPendingInvitations(int userId)
        {
            var invitations = await _context.Friendships
                .Where(f => f.UserId == userId && !f.IsAccepted) // Zaproszenia, które nie zostały zaakceptowane
                .Include(f => f.User)
                .Select(f => new FriendshipDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    FriendId = f.FriendId,
                    UserName = f.User.Username
                })
                .ToListAsync();

            return Ok(invitations);
        }

        // POST: api/Friendship/accept/{friendshipId}
        [HttpPost("accept/{friendshipId}")]
        public async Task<IActionResult> AcceptInvitation(int friendshipId)
        {
            var friendship = await _context.Friendships.FindAsync(friendshipId);

            if (friendship == null)
            {
                return NotFound("Zaproszenie nie istnieje.");
            }

            if (friendship.IsAccepted)
            {
                return BadRequest("Zaproszenie zostało już zaakceptowane.");
            }

            friendship.IsAccepted = true;
            await _context.SaveChangesAsync();

            return Ok("Zaproszenie zostało zaakceptowane.");
        }

        // DELETE: api/Friendship/reject/{friendshipId}
        [HttpDelete("reject/{friendshipId}")]
        public async Task<IActionResult> RejectInvitation(int friendshipId)
        {
            var friendship = await _context.Friendships.FindAsync(friendshipId);

            if (friendship == null)
            {
                return NotFound("Zaproszenie nie istnieje.");
            }

            if (friendship.IsAccepted)
            {
                return BadRequest("Nie można odrzucić zaproszenia, które zostało już zaakceptowane.");
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return Ok("Zaproszenie zostało odrzucone.");
        }

        // Metoda do wyszukiwania użytkowników z filtrowaniem po nazwisku i imieniu
        // GET: api/Friendship/search-users?query=John
        [HttpGet("search-users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(string query = "")
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                // Filtrowanie po imieniu lub nazwisku
                usersQuery = usersQuery.Where(u =>
                    EF.Functions.Like(u.Username, $"%{query}%") ||
                    EF.Functions.Like(u.Email, $"%{query}%"));
            }

            var users = await usersQuery
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("invite")]
        public async Task<IActionResult> SendFriendInvite([FromBody] CreateFriendshipDto createFriendshipDto)
        {
            if (createFriendshipDto.UserId == createFriendshipDto.FriendId)
            {
                return BadRequest("User cannot be friends with themselves.");
            }

            // Check if the friendship already exists
            var existingFriendship = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.UserId == createFriendshipDto.UserId && f.FriendId == createFriendshipDto.FriendId) ||
                    (f.UserId == createFriendshipDto.FriendId && f.FriendId == createFriendshipDto.UserId));

            if (existingFriendship != null)
            {
                return Conflict("Friendship already exists.");
            }

            // Create a new friendship invitation in the database
            var friendship = new Friendship
            {
                UserId = createFriendshipDto.UserId,
                FriendId = createFriendshipDto.FriendId
            };
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            // Send a notification to the friend (recipient)
            var inviter = await _context.Users.FindAsync(createFriendshipDto.UserId);
            if (inviter != null)
            {
                await _hubContext.Clients.User(createFriendshipDto.FriendId.ToString())
                    .SendAsync("ReceiveFriendInvite", createFriendshipDto.FriendId, inviter.Username);
            }

            return Ok("Friendship invitation sent.");
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
