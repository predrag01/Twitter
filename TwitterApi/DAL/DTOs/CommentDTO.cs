using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class CommentDTO
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
    }
    public class CreateCommentDTO
    {
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

    }
    public class CommentUpdateDTO
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
    }

    public class AllCommentsDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; } 
        public string CommentContent { get; set; }
    }
}
