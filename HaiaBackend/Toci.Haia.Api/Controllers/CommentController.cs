using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Api;
using Toci.Haia.ChatGPT;
using Toci.Haia.Database.Persistence;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ComedyDbContext _context;
    private readonly IMapper _mapper;
    private readonly IChatGptService _chatGptService;

    public CommentController(ComedyDbContext context, IMapper mapper, IChatGptService chatGptService)
    {
        _context = context;
        _mapper = mapper;
        _chatGptService = chatGptService;
    }

    // GET: api/Comment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
    {
        var comments = await _context.Comments
            .Include(u => u.User)
            .ToListAsync();

        //var commentDtos = _mapper.Map<List<CommentDto>>(comments);
        return Ok(comments);
    }

    [HttpGet("postComments/{postId}/")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsForPost(int postId)
    {
        var comments = await _context.Comments.Where(c => c.PostId == postId)
            .Include(u => u.User)
            .ToListAsync();
       // var commentDtos = _mapper.Map<List<CommentDto>>(comments);
        return Ok(comments);
    }

    // GET: api/Comment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        var commentDto = _mapper.Map<CommentDto>(comment);
        return Ok(commentDto);
    }

    // POST: api/Comment
    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment([FromBody] CommentDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = _mapper.Map<Comment>(commentDto);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        comment.User = _context.Users.FirstOrDefault(u => u.Id == commentDto.UserId);

       // var createdCommentDto = _mapper.Map<CommentDto>(comment);
        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    // PUT: api/Comment/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
    {
        if (id != commentDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _mapper.Map(commentDto, comment);
        _context.Entry(comment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Comment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Nowy endpoint: Generowanie żartu z ChatGPT dla komentarza
    [HttpPost("{id}/generate-joke")]
    public async Task<IActionResult> GenerateJoke(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        var joke = await _chatGptService.GenerateJokeAsync(comment.Text);
        var commentDto = _mapper.Map<CommentDto>(comment);
        


        GptJoke gptJoke = new GptJoke() { JokeText = joke, ReferenceId  = comment.Id, ReferenceKind = Util.ReferenceKindComment, RequestingUserId = comment.UserId, UserId = comment.UserId };

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

    [HttpGet("{id}/likes")]
    public async Task<ActionResult<int>> GetLikesCount(int id)
    {
        var likesCount = await _context.Likes.CountAsync(l => l.CommentId == id);
        return Ok(likesCount);
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByPost(int postId)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User) // Łączenie z tabelą User, aby uzyskać dane użytkownika
            .Select(c => new CommentResponseDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                UserName = c.User.Username, // Zakładamy, że User ma pole Username
                Content = c.Text,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(comments);
    }

   
}
