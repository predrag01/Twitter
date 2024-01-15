using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class FollowUnfollow
    {
        public int FollowingId { get; set; }
        public int FollowedId { get; set; }
        public bool Following { get; set; }
    }
}
