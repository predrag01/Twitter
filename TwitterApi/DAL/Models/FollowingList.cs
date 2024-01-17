using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class FollowingList
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int FollowerId { get; set; }
        public User? Follower { get; set; }
        [Required]
        public int FollowedId { get; set; }
        public User? Followed { get; set; }
    }
}
