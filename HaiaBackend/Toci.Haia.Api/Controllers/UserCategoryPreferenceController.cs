using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class UserCategoryPreferenceController : ControllerBase
{
    private readonly ComedyDbContext _context;

    public UserCategoryPreferenceController(ComedyDbContext context)
    {
        _context = context;
    }

    // GET: api/UserPreferences
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserPreferencesDto>>> GetUserPreferences()
    {
        var preferences = await _context.UserPreferences
            .Select(up => new UserPreferencesDto
            {
                Id = up.Id,
                UserId = up.UserId,
                CategoryId = up.CategoryId,
                PreferenceLevel = up.PreferenceLevel
            })
            .ToListAsync();

        return Ok(preferences);
    }

    // POST: api/UserPreferences
    [HttpPost]
    public async Task<ActionResult<UserPreferencesDto>> CreateUserPreference(UserPreferencesDto preferencesDto)
    {
        var preference = new UserPreferences
        {
            UserId = preferencesDto.UserId,
            CategoryId = preferencesDto.CategoryId,
            PreferenceLevel = preferencesDto.PreferenceLevel
        };

        _context.UserPreferences.Add(preference);
        await _context.SaveChangesAsync();

        preferencesDto.Id = preference.Id;
        return CreatedAtAction(nameof(GetUserPreferences), new { id = preference.Id }, preferencesDto);
    }

    // DELETE: api/UserPreferences/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserPreference(int id)
    {
        var preference = await _context.UserPreferences.FindAsync(id);
        if (preference == null)
        {
            return NotFound();
        }

        _context.UserPreferences.Remove(preference);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/UserCategoryPreference/TopCategories/{userId}
    [HttpGet("TopCategories/{userId}")]
    public async Task<ActionResult<IEnumerable<Category>>> GetTopCategories(int userId)
    {
        var topCategories = await _context.UserCategoryPreferences
            .Where(p => p.UserId == userId && p.ReactionCount > 0)
            .OrderByDescending(p => p.ReactionCount)
            .Select(p => p.Category)
            .ToListAsync();

        return Ok(topCategories);
    }


    // POST: api/UserCategoryPreference/React
    [HttpPost("React")]
    public async Task<IActionResult> ReactToCategory([FromBody] UserReactionDto reactionDto)
    {
        var preference = await _context.UserCategoryPreferences
            .FirstOrDefaultAsync(p => p.UserId == reactionDto.UserId && p.CategoryId == reactionDto.CategoryId);

        if (preference == null)
        {
            // Tworzymy nowy rekord, jeśli użytkownik nie ma jeszcze preferencji dla tej kategorii
            preference = new UserCategoryPreference
            {
                UserId = reactionDto.UserId,
                CategoryId = reactionDto.CategoryId,
                ReactionCount = reactionDto.IsPositive ? 1 : 0
            };
            _context.UserCategoryPreferences.Add(preference);
        }
        else
        {
            // Zwiększamy ReactionCount jeśli już istnieje
            preference.ReactionCount += reactionDto.IsPositive ? 1 : -1;
            if (preference.ReactionCount < 0) preference.ReactionCount = 0; // Reakcje nie mogą być ujemne
            _context.Entry(preference).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}