using DAL.DataContext;
using DAL.DTOs;
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
        private TwitterContext _db;
        public LikeRepository(TwitterContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Like> GetLikeByPostIdUserId(int userId, int postId)
        {
            var res = await _db.Likes.Where(x => x.UserId == userId && x.PostId == postId).FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<Like>> GetLikesByPostId(int postId)
        {
            var likes = await this._db.Likes.Where(x=>x.PostId == postId).ToListAsync();
            return likes;
        }

        public async Task<Like> Like(Like like)
        {
            this._db.Likes.Add(like);
            like.ID = this._db.SaveChanges();
            return like;
        }

        public async Task<Like> Unlike(Like like)
        {
            this._db.Likes.Remove(like);
            return like;
        }
    }
}
