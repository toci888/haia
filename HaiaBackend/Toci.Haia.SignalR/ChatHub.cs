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
}
