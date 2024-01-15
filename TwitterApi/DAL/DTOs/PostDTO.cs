using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class PostDTO
    {   
        public string Content { get; set; }
        public DateTime Posted { get; set; }
        public int LikeCounter { get; set; }
    }

    public class CreatePostDTO
    {
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int LikeCounter { get; set; }

    }
    public class PostUpdateDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }

    public class AllPostsDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }

}
