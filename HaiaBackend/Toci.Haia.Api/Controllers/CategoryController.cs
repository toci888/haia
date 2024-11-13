using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

namespace Toci.Haia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ComedyDbContext _context;

        public CategoryController(ComedyDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound("Kategoria nie została znaleziona.");
            }

            return Ok(new Category
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            categoryDto.Id = category.Id;

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest("ID kategorii nie jest zgodne z przekazanym obiektem.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Kategoria nie została znaleziona.");
            }

            category.Name = categoryDto.Name;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Kategoria nie została znaleziona.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
