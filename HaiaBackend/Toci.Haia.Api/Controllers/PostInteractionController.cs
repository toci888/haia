using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class PostInteractionController : ControllerBase
{
    private readonly ComedyDbContext _context;

    public PostInteractionController(ComedyDbContext context)
    {
        _context = context;
    }
    [HttpGet("suggested/{userId}")]
    public async Task<ActionResult<IEnumerable<PostJokeDto>>> GetSuggestedPostsApi(int userId)
    {
        var suggestedPosts = await GetSuggestedPosts(userId);
        return Ok(suggestedPosts);
    }

    protected async Task<List<PostJokeDto>> GetSuggestedPosts(int userId)
    {
        // Pobierz ostatnie posty, na których użytkownik spędził najwięcej czasu
        var topCategories = await _context.PostInteractions
            .Where(pi => pi.UserId == userId)
            .GroupBy(pi => pi.Post.CategoryId) // Zakładamy, że Post ma kategorię
            .OrderByDescending(g => g.Sum(pi => pi.TimeSpentMilliseconds))
            .Select(g => g.Key)
            .Take(3) // Wybieramy 3 najbardziej interesujące kategorie
            .ToListAsync();

        // Pobierz posty w tych kategoriach, pomijając te, które użytkownik już widział
        var suggestedPosts = await _context.Posts
            .Where(p => topCategories.Contains(p.CategoryId) &&
                        !_context.PostInteractions.Any(pi => pi.UserId == userId && pi.PostId == p.Id))
            .Include(gr => gr.Group)
            .Include(us => us.User)
            .Include(cat => cat.Category)
            .Include(comm => comm.Comments)
            .Include(r => r.Reactions)
            .Take(20)
            .ToListAsync();

        var suggestedJokes = await _context.Jokes
            .Where(p => topCategories.Contains(p.CategoryId) &&
                        !_context.PostInteractions.Any(pi => pi.UserId == userId && pi.PostId == p.Id))
            
            .Include(us => us.User)
            .Include(cat => cat.Category)
            .Include(comm => comm.Comments)
            .Include(r => r.Reactions)
            .Take(20)
            .ToListAsync();

        List<PostJokeDto> result = new List<PostJokeDto>();

        foreach (var element in suggestedJokes)
        {
            result.Add(MapJokeToDto(element));
        }

        foreach (var element in suggestedPosts)
        {
            result.Add(MapJokeToDto(element));
        }

        return result.OrderBy(m => m.CreatedAt).ToList();
    }

    protected PostJokeDto MapJokeToDto(Joke joke)
    {
        var result = new PostJokeDto();

        result.Id = joke.Id;
        result.UserId = joke.UserId;
        
        result.Reactions = joke.Reactions;
        result.Comments = joke.Comments;
        result.User = joke.User;
        result.CreatedAt = joke.CreatedAt;
        result.Content = joke.Text;
        result.Comments = joke.Comments;
        result.Reactions = joke.Reactions;
        result.JokeId = joke.Id;
        result.CategoryId = joke.CategoryId;
        result.Category = joke.Category;

        return result;
    }

    protected PostJokeDto MapJokeToDto(GroupPost post)
    {
        var result = new PostJokeDto();

        result.Id = post.Id;
        result.UserId = post.UserId;
        //result.Reactions = post.Reactions;
        result.Comments = post.Comments;
        result.User = post.User;
        result.CreatedAt = post.CreatedAt;
        result.Content = post.Content;
        result.Comments = post.Comments;
        result.Reactions = post.Reactions;
        result.CategoryId = post.CategoryId;
        result.Category = post.Category;
        result.Group = post.Group;

        return result;
    }

    // POST: api/PostInteraction
    [HttpPost]
    public async Task<IActionResult> LogInteraction([FromBody] PostInteractionDto interactionDto)
    {
        var interaction = new PostInteraction
        {
            PostId = interactionDto.PostId,
            UserId = interactionDto.UserId,
            TimeSpentMilliseconds = interactionDto.TimeSpentMilliseconds,
            InteractionDate = DateTime.UtcNow
        };

        _context.PostInteractions.Add(interaction);
        await _context.SaveChangesAsync();

        return Ok();
    }
}