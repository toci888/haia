using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class JokesController : ControllerBase
{
    private readonly ComedyDbContext _context;

    public JokesController(ComedyDbContext context)
    {
        _context = context;
    }

    [HttpPost("{jokeId}/react")]
    public async Task<IActionResult> ReactToJoke(int jokeId, [FromBody] ReactionDto reactionDto)
    {
        var reaction = new Reaction
        {
            JokeId = jokeId,
            UserId = reactionDto.UserId,
            ReactionType = reactionDto.ReactionType
        };
        _context.Reactions.Add(reaction);
        await _context.SaveChangesAsync();

        return Ok(reaction);
    }
}