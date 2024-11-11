using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly ComedyDbContext _context;

        public UserGroupController(ComedyDbContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }


        // GET: api/UserGroup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroupResponseDto>>> GetGroups()
        {
            var groups = await _context.UserGroups
                .Include(g => g.Users)
                .Select(g => new UserGroupResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    UserNames = g.Users.Select(u => u.Username).ToList()
                })
                .ToListAsync();

            return Ok(groups);
        }

        // GET: api/UserGroup/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupResponseDto>> GetGroup(int id)
        {
            var group = await _context.UserGroups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            var response = new UserGroupResponseDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                UserNames = group.Users.Select(u => u.Username).ToList()
            };

            return Ok(response);
        }

        // POST: api/UserGroup
        [HttpPost]
        public async Task<ActionResult<UserGroupResponseDto>> CreateGroup(UserGroupDto groupDto)
        {
            var group = new UserGroup
            {
                Name = groupDto.Name,
                Description = groupDto.Description
            };

            // Dodanie użytkowników do grupy
            group.Users = await _context.Users
                .Where(u => groupDto.UserIds.Contains(u.Id))
                .ToListAsync();

            _context.UserGroups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        [HttpPost("{groupId}/posts")]
        public async Task<ActionResult<PostResponseDto>> CreatePost(int groupId, PostDto postDto)
        {
            var group = await _context.UserGroups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var user = await _context.Users.FindAsync(postDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var category = await _context.Categories.FindAsync(postDto.CategoryId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var post = new GroupPost
            {
                GroupId = groupId,
                UserId = postDto.UserId,
                Content = postDto.Content,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var response = new PostResponseDto
            {
                Id = post.Id,
                GroupId = post.GroupId,
                UserId = post.UserId,
                UserName = user.Username,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };

            return CreatedAtAction(nameof(GetPostsByGroup), new { groupId = groupId }, response);
        }


        // PUT: api/UserGroup/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, UserGroupDto groupDto)
        {
            var group = await _context.UserGroups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = groupDto.Name;
            group.Description = groupDto.Description;

            // Aktualizacja użytkowników w grupie
            group.Users = await _context.Users
                .Where(u => groupDto.UserIds.Contains(u.Id))
                .ToListAsync();

            _context.Entry(group).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/UserGroup/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.UserGroups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.UserGroups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("{groupId}/posts")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPostsByGroup(int groupId)
        {
            var posts = await _context.Posts
                .Where(p => p.GroupId == groupId)
                .Include(p => p.User)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    GroupId = p.GroupId,
                    UserId = p.UserId,
                    UserName = p.User.Username, // Zakładam, że User ma właściwość Name
                    Content = p.Content,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(posts);
        }

        // POST: api/UserGroup/{groupId}/posts
        [HttpPost("{groupId}/posts")]
        public async Task<ActionResult<PostResponseDto>> CreatePostGroup(int groupId, PostDto postDto)
        {
            var group = await _context.UserGroups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var user = await _context.Users.FindAsync(postDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var post = new GroupPost
            {
                GroupId = groupId,
                UserId = postDto.UserId,
                Content = postDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var response = new PostResponseDto
            {
                Id = post.Id,
                GroupId = post.GroupId,
                UserId = post.UserId,
                UserName = user.Username,
                Content = post.Content,
                CreatedAt = post.CreatedAt
            };

            return CreatedAtAction(nameof(GetPostsByGroup), new { groupId = groupId }, response);
        }
    }
}
