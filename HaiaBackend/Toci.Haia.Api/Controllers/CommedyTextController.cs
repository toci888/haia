using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence;

[ApiController]
[Route("api/[controller]")]
public class ComedyTextController : ControllerBase
{
    private readonly ComedyDbContext _context;
    private readonly IMapper _mapper;

    public ComedyTextController(ComedyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/ComedyText
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComedyTextDto>>> GetComedyTexts()
    {
        var comedyTexts = await _context.ComedyTexts.Include(ct => ct.Comments).ToListAsync();
        var comedyTextDtos = _mapper.Map<List<ComedyTextDto>>(comedyTexts);
        return Ok(comedyTextDtos);
    }

    // GET: api/ComedyText/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ComedyTextDto>> GetComedyText(int id)
    {
        var comedyText = await _context.ComedyTexts.Include(ct => ct.Comments)
                                                   .FirstOrDefaultAsync(ct => ct.Id == id);
        if (comedyText == null)
        {
            return NotFound();
        }

        var comedyTextDto = _mapper.Map<ComedyTextDto>(comedyText);
        return Ok(comedyTextDto);
    }

    // POST: api/ComedyText
    [HttpPost]
    public async Task<ActionResult<ComedyTextDto>> CreateComedyText([FromBody] ComedyTextDto comedyTextDto)
    {
        var comedyText = _mapper.Map<ComedyText>(comedyTextDto);
        _context.ComedyTexts.Add(comedyText);
        await _context.SaveChangesAsync();

        var createdComedyTextDto = _mapper.Map<ComedyTextDto>(comedyText);
        return CreatedAtAction(nameof(GetComedyText), new { id = comedyText.Id }, createdComedyTextDto);
    }

    // PUT: api/ComedyText/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComedyText(int id, [FromBody] ComedyTextDto comedyTextDto)
    {
        if (id != comedyTextDto.Id)
        {
            return BadRequest();
        }

        var comedyText = await _context.ComedyTexts.FindAsync(id);
        if (comedyText == null)
        {
            return NotFound();
        }

        _mapper.Map(comedyTextDto, comedyText);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ComedyText/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComedyText(int id)
    {
        var comedyText = await _context.ComedyTexts.FindAsync(id);
        if (comedyText == null)
        {
            return NotFound();
        }

        _context.ComedyTexts.Remove(comedyText);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
