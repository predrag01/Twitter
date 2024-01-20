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
            var key = $"comment:{id}";
            var cachedComment = await redis.StringGetAsync(key);

            if (!cachedComment.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<Comment>(cachedComment);
            }
            else
            {
                var com = await _db.Comments.Where(x => x.ID == id).FirstOrDefaultAsync();

                if (com != null)
                {
                    await redis.StringSetAsync(key, JsonConvert.SerializeObject(com));
                }

                return com;
            }
        }

        public async Task<Comment> CreateComment(Comment com)
        {
            this._db.Comments.Add(com);
            com.ID = _db.SaveChanges();

            var redis = _redis.GetDatabase();

            var key = $"comment:{com.ID}";
            await redis.StringSetAsync(key, JsonConvert.SerializeObject(com));

            return com;
        }
        public async Task<Comment> UpdateComment(Comment com)
        {
            this._db.Comments.Update(com);

            var redis = _redis.GetDatabase();

            var key = $"comment:{com.ID}";
            await redis.StringSetAsync(key, JsonConvert.SerializeObject(com));

            return com;
        }

        public async Task<Comment> DeleteComment(Comment com)
        {
            this._db.Comments.Remove(com);

            var redis = _redis.GetDatabase();

            var key = $"comment:{com.ID}";
            await redis.KeyDeleteAsync(key);

            return com;
        }
        public async Task<List<Comment>> GetAllCommentsByPostId(int postId)
        {
            var redis = _redis.GetDatabase();

            var key = $"commentsByPost:{postId}";
            var cachedComments = await redis.StringGetAsync(key);

            if (!cachedComments.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<List<Comment>>(cachedComments);
            }
            else
            {
                var comments = await this._db.Comments.Include(p => p.User).Where(x => x.PostId == postId).ToListAsync();

                if (comments != null && comments.Count > 0)
                {
                    await redis.StringSetAsync(key, JsonConvert.SerializeObject(comments));
                }

                return comments;
            }
        }
    }
    
}

