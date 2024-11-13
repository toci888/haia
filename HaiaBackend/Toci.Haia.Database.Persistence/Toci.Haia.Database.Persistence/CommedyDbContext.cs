using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toci.Haia.Database.Persistence
{
    public class ComedyDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Toci.Haia;Username=postgres;Password=beatka");

            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<SocialLogin> SocialLogins { get; set; }

        public DbSet<Joke> Jokes { get; set; }
        public DbSet<GptJoke> GptJokes { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPost> Posts { get; set; } // Dodane posty
        public DbSet<Category> Categories { get; set; } // Dodane kategorie
        public DbSet<PostInteraction> PostInteractions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        public DbSet<ComedyText> ComedyTexts { get; set; }
        public DbSet<UserCategoryPreference> UserCategoryPreferences { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji jeden-do-wielu między User a UserPreferences
            modelBuilder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne()
                .HasForeignKey<UserPreferences>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji wiele-do-jednego między UserPreferences a Category
            modelBuilder.Entity<UserPreferences>()
                .HasOne(up => up.Category);
                //.WithMany(c => c.UserPreferences)
                //.HasForeignKey(up => up.CategoryId);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(cr => cr.Messages)
                .WithOne(cm => cm.ChatRoom)
                .HasForeignKey(cm => cm.ChatRoomId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(cm => cm.User)
                .HasForeignKey(cm => cm.UserId);

            modelBuilder.Entity<UserCategoryPreference>()
                .HasOne(ucp => ucp.User);
                //.WithMany(u => u.UserCategoryPreferences)
                //.HasForeignKey(ucp => ucp.UserId);

                modelBuilder.Entity<UserCategoryPreference>()
                    .HasOne(ucp => ucp.Category);
            //.WithMany(c => c.UserCategoryPreferences)
            //.HasForeignKey(ucp => ucp.CategoryId);

            modelBuilder.Entity<UserGroup>();
                //.HasMany(g => g.Posts)
                //.WithOne(p => p.Group)
                //.HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<Category>();
                //.HasMany(c => c.Posts);
                //.WithOne(p => p.CategoryId)
                //.HasForeignKey(p => p.CategoryId);


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Satyra" },
                new Category { Id = 2, Name = "Parodia" },
                new Category { Id = 3, Name = "Ironia" },
                new Category { Id = 4, Name = "Humor czarny" },
                new Category { Id = 5, Name = "Humor absurdalny" },
                new Category { Id = 6, Name = "Słowna gra" },
                new Category { Id = 7, Name = "Karykatura" },
                new Category { Id = 8, Name = "Humor polityczny" }
            );


            // Definicje relacji i kluczy obcych
            modelBuilder.Entity<UserGroup>();
                //.HasMany(g => g.Posts)
                //.WithOne(p => p.Group)
                //.HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<UserGroup>()
                .HasMany(g => g.Users);
                //.WithMany(u => u.use);

            //modelBuilder.Entity<User>()
            //    .HasKey(u => u.Id);

            // Unikalny adres e-mail dla User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relacja jeden-do-wielu: User -> SocialLogins
            modelBuilder.Entity<User>()
                .HasMany(u => u.SocialLogins)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie użytkownika usuwa logowania społecznościowe

            // Konfiguracja tabeli SocialLogin
            modelBuilder.Entity<SocialLogin>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SocialLogin>()
                .Property(s => s.Provider)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<SocialLogin>()
                .Property(s => s.ProviderUserId)
                .IsRequired();

            // Unikalne połączenie dostawcy i jego ID, aby uniknąć duplikacji
            modelBuilder.Entity<SocialLogin>()
                .HasIndex(s => new { s.Provider, s.ProviderUserId })
                .IsUnique();

            // Konfiguracja tabeli Joke
            modelBuilder.Entity<Joke>()
                .HasKey(j => j.Id);  // Ustawienie klucza głównego

            modelBuilder.Entity<Joke>()
                .Property(j => j.Text)
                .IsRequired()
                .HasMaxLength(500);  // Maksymalna długość dowcipu

            modelBuilder.Entity<Joke>()
                .HasMany(j => j.Reactions);      // Relacja jeden-do-wielu
                //.WithOne(r => r.Joke)            // Reakcja odnosi się do jednego dowcipu
                //.HasForeignKey(r => r.JokeId)    // Klucz obcy
                //.OnDelete(DeleteBehavior.Cascade);  // Usunięcie dowcipu usuwa też reakcje

            // Konfiguracja tabeli Reaction
            modelBuilder.Entity<Reaction>()
                .HasKey(r => r.Id);  // Ustawienie klucza głównego

            modelBuilder.Entity<Reaction>()
                .Property(r => r.ReactionType)
                .IsRequired()
                .HasMaxLength(20);  // Maksymalna długość dla typu reakcji, np. "like", "superlike"

            //modelBuilder.Entity<Reaction>()
            //    //.HasOne(r => r.Joke)        // Każda reakcja jest na jeden dowcip
            //    //.WithMany(j => j.Reactions) // Dowcip może mieć wiele reakcji
            //    .HasForeignKey(r => r.JokeId)
            //    .OnDelete(DeleteBehavior.Cascade);  // Usunięcie dowcipu usuwa też reakcje

            // Unikalne ograniczenie: Użytkownik może mieć tylko jedną reakcję na dowcip
            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.JokeId })
                .IsUnique();

            // Seedowanie danych przykładowych dla Joke
            modelBuilder.Entity<Joke>().HasData(
                new Joke { Id = 1, Text = "Dlaczego niebo jest niebieskie? Bo programista jeszcze nie skończył debugować!" },
                new Joke { Id = 2, Text = "Dlaczego komputer był smutny? Bo miał zbyt dużo problemów!" }
            );

            // Seedowanie danych przykładowych dla Reaction
            modelBuilder.Entity<Reaction>().HasData(
                new Reaction { Id = 1, JokeId = 1, UserId = 1, ReactionType = "like" },
                new Reaction { Id = 2, JokeId = 1, UserId = 2, ReactionType = "superlike" },
                new Reaction { Id = 3, JokeId = 2, UserId = 1, ReactionType = "meh" }
            );

            //modelBuilder.Entity<Reaction>()
            //    .HasOne(r => r.Joke)
            //    .WithMany(j => j.Reactions)
            //    .HasForeignKey(r => r.JokeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);


    

            modelBuilder.Entity<ComedyText>()
        .HasOne(ct => ct.ChildComedyText)       // ComedyText ma jedno ChildComedyText
        .WithOne(ct => ct.ParentComedyText)     // ChildComedyText ma jedno ParentComedyText
        .HasForeignKey<ComedyText>(ct => ct.Id);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Comment)
                .WithMany()
                .HasForeignKey(l => l.CommentId);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId);

        //    modelBuilder.Entity<User>().HasData(
        //    Enumerable.Range(1, 10).Select(i => new User
        //    {
        //        Id = i,
        //        Username = $"User_{i}",
        //        Email = $"user{i}@example.com",
        //        PasswordHash = $"hash_user_{i}"
        //    }).ToArray()
        //);

            // Seedowanie UserProfile
            modelBuilder.Entity<UserProfile>().HasData(
                Enumerable.Range(1, 10).Select(i => new UserProfile
                {
                    Id = i,
                    Interests = $"Interests_{i}",
                    ProfilePictureUrl = $"https://example.com/user{i}.jpg"
                }).ToArray()
            );

            // Seedowanie ComedyText
            modelBuilder.Entity<ComedyText>().HasData(
                Enumerable.Range(1, 10).Select(i => new ComedyText
                {
                    Id = i,
                    Text = $"Sample Comedy Text {i}",
                    Author = $"Author_{i}",
                    CommentTimestamp = DateTime.UtcNow,
                    ParentTextId = null,
                    ChildTextId = null
                }).ToArray()
            );

            // Seedowanie Comment
            modelBuilder.Entity<Comment>().HasData(
                Enumerable.Range(1, 10).Select(i => new Comment
                {
                    Id = i,
                    Text = $"Sample Comment {i}",
                   // Author = $"Commenter_{i}",
              
                    CommentTimestamp = DateTime.UtcNow,
         
                }).ToArray()
            );

            // Seedowanie Like
            modelBuilder.Entity<Like>().HasData(
                Enumerable.Range(1, 10).Select(i => new Like
                {
                    Id = i,
                    UserId = i,
                    CommentId = i
                }).ToArray()
            );

            // Seedowanie Friendship
            modelBuilder.Entity<Friendship>().HasData(
                Enumerable.Range(1, 10).Select(i => new Friendship
                {
                    Id = i,
                    UserId = i,
                    FriendId = (i % 10) + 1 // Przykładowe przypisanie przyjaźni w kółko
                }).ToArray()
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "user1", Email = "user1@example.com", PasswordHash = "hashed_password_1" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com", PasswordHash = "hashed_password_2" }
            );

            modelBuilder.Entity<SocialLogin>().HasData(
                new SocialLogin { Id = 1, Provider = "Google", ProviderUserId = "google_user_1", UserId = 1 },
                new SocialLogin { Id = 2, Provider = "Facebook", ProviderUserId = "facebook_user_1", UserId = 1 },
                new SocialLogin { Id = 3, Provider = "GitHub", ProviderUserId = "github_user_2", UserId = 2 }
            );
        }

    }

}
