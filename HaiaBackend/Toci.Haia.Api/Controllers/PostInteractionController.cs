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
    public async Task<ActionResult<IEnumerable<GroupPost>>> GetSuggestedPostsApi(int userId)
    {
        var suggestedPosts = await GetSuggestedPosts(userId);
        return Ok(suggestedPosts);
    }

    protected async Task<List<GroupPost>> GetSuggestedPosts(int userId)
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
            .Take(5)
            .ToListAsync();

        return suggestedPosts;
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