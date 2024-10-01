using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ComedyDbContext _context;
    private readonly IMapper _mapper;

    public LikeController(ComedyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Like>> LikeComment([FromBody] LikeDTO likeDto)
    {
        var like = _mapper.Map<Like>(likeDto);

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
        FriendRequestHub frHub = new FriendRequestHub(_context);    

        // Automatically invite users who liked the same comment
        var likers = await _context.Likes
            .Where(l => l.CommentId == like.CommentId && l.UserId != like.UserId)
            .Select(l => l.UserId)
            .ToListAsync();

        foreach (var userId in likers)
        {
            // Automatyczne zapraszanie do znajomych
            var friendship = new Friendship
            {
                UserId = like.UserId,
                FriendId = userId
            };
            _context.Friendships.Add(friendship);

            frHub.SendFriendRequest(like.UserId.ToString(), "Hej, tu supa doopa !");
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLike", new { id = like.Id }, like);
    }

    // Other CRUD operations...
}
