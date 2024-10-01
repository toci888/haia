using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
    {
        var comments = await _context.Comments.ToListAsync();
        var commentDtos = _mapper.Map<List<CommentDto>>(comments);
        return Ok(commentDtos);
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
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CommentDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = _mapper.Map<Comment>(commentDto);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var createdCommentDto = _mapper.Map<CommentDto>(comment);
        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, createdCommentDto);
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
        commentDto.GptJoke = joke;

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

        return Ok(commentDto);
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
}
