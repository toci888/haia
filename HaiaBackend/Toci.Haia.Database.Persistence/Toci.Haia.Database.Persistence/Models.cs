using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toci.Haia.Database.Persistence
{
    public class Joke
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; internal set; }
    }

    public class Reaction
    {
        public int Id { get; set; }
        public string ReactionType { get; set; }
        public int? JokeId { get; set; }
        public Joke Joke { get; set; }
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
        public int UserId { get; set; }
    }


    public class Comment
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

        public string GptJoke { get; set; }
        public ComedyText ComedyText { get; set; }

        // Łańcuchowanie – referencja do "rodzica" i "dziecka"
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; }
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




}
