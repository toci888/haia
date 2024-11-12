using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toci.Haia.Api;
using AutoMapper;
using Toci.Haia.ChatGPT;

[Route("api/[controller]")]
[ApiController]
public class JokesController : ControllerBase
{
    private readonly ComedyDbContext _context;
    private readonly IMapper _mapper;
    private readonly IChatGptService _chatGptService;

    public JokesController(ComedyDbContext context, IMapper mapper, IChatGptService chatGptService)
    {
        _context = context;
        _mapper = mapper;
        _chatGptService = chatGptService;
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
            Text = jokeDto.Text,
            UserId = jokeDto.UserId,
            CategoryId = jokeDto.CategoryId
        };

        _context.Jokes.Add(joke);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJokeById), new { id = joke.Id }, joke);
    }

    // Nowy endpoint: Generowanie żartu z ChatGPT dla zartu
    [HttpPost("{id}/generate-joke")]
    public async Task<IActionResult> GenerateJoke(int id)
    {
        var joke = await _context.Jokes.FindAsync(id);

        if (joke == null)
        {
            return NotFound();
        }

        var gptJokeText = await _chatGptService.GenerateJokeAsync(joke.Text);

 
        GptJoke gptJoke = new GptJoke() { JokeText = gptJokeText, ReferenceId = joke.Id, ReferenceKind = Util.ReferenceKindJoke, UserId = joke.UserId, RequestingUserId = joke.UserId };

        _context.GptJokes.Add(gptJoke);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }

        return Ok(gptJoke);
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

    // Endpoint do pobrania wszystkich dowcipów
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Joke>>> GetAllJokes()
    {
        var jokes = await _context.Jokes.Include(j => j.User).
            Include(c => c.Comments).
            Include(c => c.Reactions).
            ToListAsync();



        return Ok(jokes);
    }

    // Endpoint do aktualizacji dowcipu
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJoke(int id, [FromBody] JokeDto jokeDto)
    {
        var joke = await _context.Jokes.FindAsync(id);

        if (joke == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(jokeDto.Text))
        {
            return BadRequest("Treść dowcipu nie może być pusta.");
        }

        joke.Text = jokeDto.Text;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Endpoint do usunięcia dowcipu
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJoke(int id)
    {
        var joke = await _context.Jokes.FindAsync(id);
        if (joke == null)
        {
            return NotFound();
        }

        _context.Jokes.Remove(joke);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Endpoint do dodania reakcji do dowcipu
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
