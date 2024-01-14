using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Posted { get; set; } = DateTime.Now;

        [Required]
        public int AuthorId { get; set; }
        public User? Author { get; set; }
        [NotMapped]
        public int LikeCounter { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
