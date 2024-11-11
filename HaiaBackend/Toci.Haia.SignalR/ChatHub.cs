using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

public class ChatHub : Hub
{
    private readonly ComedyDbContext _dbContext;

    // Konstruktor, który wstrzykuje kontekst bazy danych
    public ChatHub(ComedyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Funkcja, która będzie wysyłać wiadomość do wszystkich klientów
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Funkcja, która pobiera dane z tabeli ComedyText i wysyła je do klientów
    public async Task SendComedyTexts()
    {
        // Pobieramy wszystkie wpisy z tabeli ComedyText
        var comedyTexts = await _dbContext.ComedyTexts.ToListAsync();

        // Wysyłamy listę tekstów do wszystkich klientów
        await Clients.All.SendAsync("ReceiveComedyTexts", comedyTexts);
    }

    public async Task SendMessage(int roomId, int userId, string message)
    {
        var chatMessage = new ChatMessage
        {
            ChatRoomId = roomId,
            UserId = userId,
            Content = message,
            Timestamp = DateTime.UtcNow
        };

        _dbContext.ChatMessages.Add(chatMessage);
        await _dbContext.SaveChangesAsync();

        // Wysyła wiadomość do wszystkich użytkowników w pokoju
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userId, message, chatMessage.Timestamp);
    }

    // Dołączanie do pokoju
    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    // Opuszczanie pokoju
    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}
