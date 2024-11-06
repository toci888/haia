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
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ComedyText> ComedyTexts { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Joke)
                .WithMany(j => j.Reactions)
                .HasForeignKey(r => r.JokeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);


            // Konfiguracja relacji jeden-do-wielu
            modelBuilder.Entity<ComedyText>()
                .HasMany(ct => ct.Comments)
                .WithOne(c => c.ComedyText)
                .HasForeignKey(c => c.ComedyTextId);

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

            modelBuilder.Entity<User>().HasData(
            Enumerable.Range(1, 10).Select(i => new User
            {
                Id = i,
                Username = $"User_{i}",
                Email = $"user{i}@example.com",
                PasswordHash = $"hash_user_{i}"
            }).ToArray()
        );

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
                    Author = $"Commenter_{i}",
                    Snippet = $"Snippet {i}",
                    SnippetAuthor = $"SnippetAuthor_{i}",
                    CommentTimestamp = DateTime.UtcNow,
                    SnippetTimestamp = DateTime.UtcNow,
                    ComedyTextId = i,
                    GptJoke = $"Generated Joke {i}",
                    ParentCommentId = null
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
        }

    }

}
