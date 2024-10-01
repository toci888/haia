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

        public int ComedyTextId { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Snippet { get; set; }
        public string SnippetAuthor { get; set; }
        public DateTime CommentTimestamp { get; set; }
        public DateTime SnippetTimestamp { get; set; }

        public string GptJoke { get; set; }
    }

    public class LikeDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }

    public class FriendshipDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }

}
