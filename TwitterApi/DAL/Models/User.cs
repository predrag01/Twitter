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
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        [Required]
        public int FollowingCount { get; set; }
        [Required]
        public int FollowedCount { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Like> Likes { get; set; }
        [JsonIgnore]
        public virtual ICollection<FollowingList> Following { get; set; }
        [JsonIgnore]
        public virtual ICollection<FollowingList> Followed { get; set; }
        [NotMapped]
        public bool CheckFollowing { get; set; }
    }
}
