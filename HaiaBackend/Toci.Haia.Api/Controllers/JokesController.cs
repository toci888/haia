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

    // Endpoint do dodawania nowego dowcipu
    [HttpPost]
    public async Task<ActionResult<Joke>> AddJoke([FromBody] JokeDto jokeDto)
    {
        if (string.IsNullOrWhiteSpace(jokeDto.Text))
        {
            return BadRequest("Treść dowcipu nie może być pusta.");
        }

        var joke = new Joke
        {
            Text = jokeDto.Text
        };

        _context.Jokes.Add(joke);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJokeById), new { id = joke.Id }, joke);
    }

    // Endpoint do pobrania dowcipu na podstawie Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Joke>> GetJokeById(int id)
    {
        var joke = await _context.Jokes.FindAsync(id);

        if (joke == null)
        {
            return NotFound();
        }

        return Ok(joke);
    }
}