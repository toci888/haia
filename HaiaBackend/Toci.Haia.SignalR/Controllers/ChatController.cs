using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ComedyDbContext _context;

    public ChatController(ComedyDbContext context)
    {
        _context = context;
    }

    // GET: api/Chat/Rooms
    [HttpGet("Rooms")]
    public async Task<ActionResult<IEnumerable<ChatRoom>>> GetChatRooms()
    {
        return await _context.ChatRooms.ToListAsync();
    }

    // POST: api/Chat/Rooms
    [HttpPost("Rooms")]
    public async Task<ActionResult<ChatRoom>> CreateChatRoom([FromBody] string roomName)
    {
        var chatRoom = new ChatRoom { Name = roomName };
        _context.ChatRooms.Add(chatRoom);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChatRooms), new { id = chatRoom.Id }, chatRoom);
    }

    // GET: api/Chat/Rooms/{roomId}/Messages
    [HttpGet("Rooms/{roomId}/Messages")]
    public async Task<ActionResult<IEnumerable<ChatMessage>>> GetMessages(int roomId)
    {
        return await _context.ChatMessages
            .Where(m => m.ChatRoomId == roomId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }
}