using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _redis;
        public LikeRepository(TwitterContext db, IConnectionMultiplexer redis) : base(db)
        {
            _db = db;
            _redis = redis;
        }

        public async Task<Like> GetLikeByPostIdUserId(int userId, int postId)
        {
            var res = await _db.Likes.Where(x => x.UserId == userId && x.PostId == postId).FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<Like>> GetLikesByPostId(int postId)
        {
            var redis = _redis.GetDatabase();

            var key = $"postLikes:{postId}";
            var redisValue = await redis.ListRangeAsync(key);
            if(redisValue.Any())
            {
                var redisLikes = redisValue.Select(item => JsonConvert.DeserializeObject<Like>(item)).ToList();
                return redisLikes;
            }

            var likes = await this._db.Likes.Where(x=>x.PostId == postId).ToListAsync();
            if(likes != null)
            {
                var serializedLikes = likes.Select(item => JsonConvert.SerializeObject(item)).ToArray();
                await redis.ListRightPushAsync(key, serializedLikes.Select(x => (RedisValue)x).ToArray());
            }

            return likes;
        }

        public async Task<Like> Like(Like like)
        {
            this._db.Likes.Add(like);
            this._db.SaveChanges();

            var redis = _redis.GetDatabase();

            var key = $"postLikes:{like.PostId}";
            await redis.ListRightPushAsync(key, JsonConvert.SerializeObject(like));

            return like;
        }

        public async Task<Like> Unlike(Like like)
        {
            var redis = _redis.GetDatabase();

            var key = $"postLikes:{like.PostId}";
            await redis.ListRemoveAsync(key, JsonConvert.SerializeObject(like));

            this._db.Likes.Remove(like);
            return like;
        }
    }
}
