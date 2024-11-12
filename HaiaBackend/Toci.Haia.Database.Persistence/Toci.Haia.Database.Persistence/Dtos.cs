using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toci.Haia.Database.Persistence
{
    public class ComedyTextDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Text cannot exceed 500 characters")]
        public string Text { get; set; }

        [Required]
        public string Author { get; set; }

        public DateTime CommentTimestamp { get; set; }
    }


    public class CommentDto
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        public int JokeId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        //public string Author { get; set; }
        public DateTime CommentTimestamp { get; set; }
    }

    public class LikeDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }

    
    // FriendshipDto.cs
    public class FriendshipDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string UserName { get; set; }
        public string FriendName { get; set; }
    }



    // CreateFriendshipDto.cs
    public class CreateFriendshipDto
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }


    public class ReactionDto
    {
        public int UserId { get; set; }           // ID of the user making the reaction
        public string ReactionType { get; set; }   // Type of reaction (e.g., "like", "superlike", etc.)
    }

    public class UserRegistrationDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<SocialLoginDto> SocialLogins { get; set; }
    }

    public class SocialLoginDto
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class JokeDto
    {
        public int JokeId { get; set; }
        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }
    }

    // UserGroupDto.cs
    public class UserGroupDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> UserIds { get; set; } // Lista ID użytkowników
    }

    // UserGroupResponseDto.cs
    public class UserGroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> UserNames { get; set; } // Lista nazw użytkowników
    }

    // PostDto.cs
    public class PostDto
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Content { get; set; }
    }

    // PostResponseDto.cs
    public class PostResponseDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PostInteractionDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int TimeSpentMilliseconds { get; set; }
        public int CategoryId { get; set; }
    }

    // UserReactionDto.cs
    public class UserReactionDto
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public bool IsPositive { get; set; } // True jeśli reakcja jest pozytywna, False jeśli negatywna
    }

}
