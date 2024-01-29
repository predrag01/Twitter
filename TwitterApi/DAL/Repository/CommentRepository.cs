using DAL.DataContext;
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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private TwitterContext _db;
        private readonly IConnectionMultiplexer _redis;
        public CommentRepository(TwitterContext db, IConnectionMultiplexer redis) : base(db)
        {
            _db = db;
            _redis = redis;
        }
        public async Task<Comment> GetCommentById(int id)
        {
            var redis = _redis.GetDatabase();

            var key = $"comment:{id}:commentId";
            var redisValue = await redis.StringGetAsync(key);

            if (!redisValue.IsNull)
            {
                var commentFromRedis = JsonConvert.DeserializeObject<Comment>(redisValue);
                return commentFromRedis;
            }

            var com = await this._db.Comments.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (com != null)
            {
                var serializedComment = JsonConvert.SerializeObject(com);
                await redis.StringSetAsync(key, serializedComment);
            }
            return com;
        }

        public async Task<Comment> CreateComment(Comment com)
        {
            this._db.Comments.Add(com);
            await this._db.SaveChangesAsync();

            var redis = _redis.GetDatabase();

            var key = $"post:{com.PostId}:comments";
            await redis.ListRightPushAsync(key, JsonConvert.SerializeObject(com));

            var newKey = $"comment:{com.ID}:commentId";
            await redis.StringSetAsync(newKey, JsonConvert.SerializeObject(com));

            return com;
        }
        public async Task<Comment> UpdateComment(Comment com)
        {
            this._db.Comments.Update(com);

            var redis = _redis.GetDatabase();

            var key = $"post:{com.PostId}:comments";
            var comfordel = await GetCommentById(com.ID);
            await redis.ListRemoveAsync(key, JsonConvert.SerializeObject(comfordel));
            await redis.ListRightPushAsync(key, JsonConvert.SerializeObject(com));

            var commKey = $"comment:{com.ID}:commentId";
            await redis.KeyDeleteAsync(commKey);            
            await redis.StringSetAsync(commKey, JsonConvert.SerializeObject(com));

            return com;
        }

        public async Task<Comment> DeleteComment(Comment com)
        {
            this._db.Comments.Remove(com);

            var redis = _redis.GetDatabase();
            var key = $"post:{com.PostId}:comments";
            await redis.ListRemoveAsync(key, JsonConvert.SerializeObject(com));

            var commKey = $"comment:{com.ID}:commentId";
            await redis.KeyDeleteAsync(commKey);

            
            return com;
        }
        public async Task<List<Comment>> GetAllCommentsByPostId(int postId)
        {
            var redis = _redis.GetDatabase();
            var key = $"post:{postId}:comments";

            var redisValues = await redis.ListRangeAsync(key);
            if (redisValues.Any())
            {
                var comsFromRedis = redisValues.Select(redisValue => JsonConvert.DeserializeObject<Comment>(redisValue)).ToList();
                return comsFromRedis;
            }
            
            List<Comment> comsFromDb = await this._db.Comments.Include(p => p.User).Where(x => x.PostId == postId).ToListAsync();
            foreach (var com in comsFromDb)
            {
                await redis.ListRightPushAsync(key, JsonConvert.SerializeObject(com));
            }

            return comsFromDb;
        }
    }
    }
