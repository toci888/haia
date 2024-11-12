using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Toci.Haia.Database.Persistence
{
    public class Joke
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }
    }

    public class GptJoke
    {
        public int Id { get; set; }

        public int ReferenceId { get; set; }

        public int ReferenceKind { get; set; }

        public string JokeText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public int RequestingUserId { get; set; }

        public User User { get; set; }
    }

    public class Reaction
    {
        public int Id { get; set; }
        public string ReactionType { get; set; }
        public int? JokeId { get; set; }
       //public Joke Joke { get; set; }
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public int GroupPostId { get; set; }
    }

    public class CommedyTextComment
    {
        public int JokeId { get; set; }
        public Joke Joke { get; set; }
        public int UserId { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Snippet { get; set; }
        public string SnippetAuthor { get; set; }
        public DateTime CommentTimestamp { get; set; }
        public DateTime SnippetTimestamp { get; set; }

        // Powiązanie z ComedyText
        public int ComedyTextId { get; set; }

        public ComedyText ComedyText { get; set; }

        // Łańcuchowanie – referencja do "rodzica" i "dziecka"
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; }
    }

    public class Comment
    {
        public int JokeId { get; set; }
        public int CommentId { get; set; }
        //public Joke Joke { get; set; }
        public int UserId { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public int Id { get; set; }
        public string Text { get; set; }
        //public string Author { get; set; }

        public DateTime CommentTimestamp { get; set; }

        public ICollection<Comment> Replies { get; set; }

        public int GroupPostId { get; set; }
    }

    public class ComedyText
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime CommentTimestamp { get; set; }

        public int? ParentTextId { get; set; }
        public ComedyText ParentComedyText { get; set; }
        public ICollection<Comment> Replies { get; set; }

        public int? ChildTextId { get; set; }
        public ComedyText ChildComedyText { get; set; }

        // Lista komentarzy
        public ICollection<Comment> Comments { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<ComedyText> ComedyTexts { get; set; } = new List<ComedyText>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }

        // Relacja z SocialLogin (opcjonalne logowanie przez zewnętrzne platformy)
        public ICollection<SocialLogin> SocialLogins { get; set; }
        public List<UserCategoryPreference> UserCategoryPreferences { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    public class SocialLogin
    {
        public int Id { get; set; }
        public string Provider { get; set; } // Platforma: Google, Facebook, Microsoft, GitHub, Apple, LinkedIn
        public string ProviderUserId { get; set; } // Unikalny ID użytkownika przyznany przez dostawcę
        public DateTime LinkedAt { get; set; } = DateTime.UtcNow; // Kiedy połączono konto

        // Relacja z tabelą User
        public int UserId { get; set; }
        public User User { get; set; }
    }


    public class UserProfile
    {
        public int Id { get; set; }
        public string Interests { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }

        public User User { get; set; }
        public Comment Comment { get; set; }
    }

    public class Friendship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public User User { get; set; }
        public User Friend { get; set; }
    }

    // UserGroup.cs
    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relacja do użytkowników
        public List<User> Users { get; set; } = new List<User>();
        //public List<GroupPost> Posts { get; set; } = new List<GroupPost>(); // Dodane posty
    }

    // Post.cs
    public class GroupPost
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacje
        public UserGroup Group { get; set; }
        public User User { get; set; }

        public Category Category { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Reaction> Reactions { get; set; }

        public int CategoryId { get; set; }

        public int GroupPostId { get; set; }
    }

    // PostInteraction.cs
    public class PostInteraction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int TimeSpentMilliseconds { get; set; } // Czas w milisekundach
        public DateTime InteractionDate { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        
        public GroupPost Post { get; set; }
    }

    // Category.cs
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Relacja z postami
        //public List<GroupPost> Posts { get; set; } = new List<GroupPost>();

        public List<UserCategoryPreference> UserCategoryPreferences { get; set; }
    }

    // Post.cs
    public class Post
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; } // Klucz obcy do kategorii
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacje
        public UserGroup Group { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
    }

    // UserCategoryPreference.cs
    public class UserCategoryPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int ReactionCount { get; set; } = 0; // Liczba reakcji użytkownika na daną kategorię

        // Relacje
        public User User { get; set; }
        public Category Category { get; set; }
    }

    // ChatRoom.cs
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } // Nazwa pokoju

        // Relacja z wiadomościami
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    // ChatMessage.cs
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; } // Klucz obcy do pokoju czatu
        public int UserId { get; set; } // Klucz obcy do użytkownika wysyłającego wiadomość
        public string Content { get; set; } // Treść wiadomości
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Czas wysłania

        // Relacje
        public ChatRoom ChatRoom { get; set; }
        public User User { get; set; }
    }

}
