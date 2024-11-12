using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly ComedyDbContext _context;

        public ReactionController(ComedyDbContext context)
        {
            _context = context;
        }

        // GET: api/Reaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReactionResponseDto>>> GetReactions()
        {
            var reactions = await _context.Reactions
                .Select(r => new ReactionResponseDto
                {
                    Id = r.Id,
                    ReactionType = r.ReactionType,
                    JokeId = r.JokeId,
                    CommentId = r.CommentId,
                    UserId = r.UserId
                })
                .ToListAsync();

            return Ok(reactions);
        }

        // GET: api/Reaction/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReactionResponseDto>> GetReaction(int id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction == null)
            {
                return NotFound();
            }

            var response = new ReactionResponseDto
            {
                Id = reaction.Id,
                ReactionType = reaction.ReactionType,
                JokeId = reaction.JokeId,
                CommentId = reaction.CommentId,
                UserId = reaction.UserId
            };

            return Ok(response);
        }

        // POST: api/Reaction
        [HttpPost]
        public async Task<ActionResult<ReactionResponseDto>> CreateReaction(ReactionDto reactionDto)
        {
            var reaction = new Reaction
            {
                ReactionType = reactionDto.ReactionType,
                JokeId = reactionDto.JokeId,
                CommentId = reactionDto.CommentId,
                UserId = reactionDto.UserId
            };

            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync();

            var response = new ReactionResponseDto
            {
                Id = reaction.Id,
                ReactionType = reaction.ReactionType,
                JokeId = reaction.JokeId,
                CommentId = reaction.CommentId,
                UserId = reaction.UserId
            };

            return CreatedAtAction(nameof(GetReaction), new { id = reaction.Id }, response);
        }

        // PUT: api/Reaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReaction(int id, ReactionDto reactionDto)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction == null)
            {
                return NotFound();
            }

            reaction.ReactionType = reactionDto.ReactionType;
            reaction.JokeId = reactionDto.JokeId;
            reaction.CommentId = reactionDto.CommentId;
            reaction.UserId = reactionDto.UserId;

            _context.Entry(reaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReaction(int id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction == null)
            {
                return NotFound();
            }

            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
