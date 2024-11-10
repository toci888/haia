using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Toci.Haia.Database.Persistence;

public class FriendRequestHub : Hub
{
    private readonly ComedyDbContext _dbContext;

    // Konstruktor, który wstrzykuje kontekst bazy danych
    public FriendRequestHub(ComedyDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    public override async Task OnDisconnectedAsync(Exception exception)
    {
        //_connectedUsers.TryRemove(Context.ConnectionId, out string userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendFriendRequest(string userId, string message)
    {
        // Wyślij powiadomienie o nowym zaproszeniu do użytkownika o danym userId
        await Clients.User(userId).SendAsync("ReceiveFriendRequest", message);
    }

    public async Task SendFriendInviteNotification(int recipientUserId, string inviterName)
    {
        // Wysyłanie powiadomienia do konkretnego użytkownika po jego Id
        await Clients.User(recipientUserId.ToString()).SendAsync("ReceiveFriendInvite", inviterName);
    }
}
