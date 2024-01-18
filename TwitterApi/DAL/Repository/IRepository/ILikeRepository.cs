using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<List<Like>> GetLikesByPostId(int postId);
        Task<Like> Like(Like like);
        Task<Like> Unlike(Like like);
        Task<Like> GetLikeByPostIdUserId(int userId, int postId);
    }
}
