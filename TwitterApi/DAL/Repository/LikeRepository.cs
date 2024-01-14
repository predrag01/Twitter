using DAL.Models;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(DbContext context) : base(context)
        {
        }
    }
}
