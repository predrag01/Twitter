using DAL.DataContext;
using DAL.Models;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private TwitterContext _db;
        private readonly IConnectionMultiplexer _redis;
        public PostRepository(TwitterContext db, IConnectionMultiplexer redis) : base(db)
        {
            _db = db;
            _redis = redis;
        }


        public async Task<Post> GetPostById(int id)
        {

            var redis = _redis.GetDatabase();
            var key = $"post:{id}:postId";
            var redisValue=await redis.StringGetAsync(key);

            if (!redisValue.IsNull)
            {
                var postFromRedis=JsonConvert.DeserializeObject<Post>(redisValue);
                return postFromRedis;
            }
            
            var post = await this._db.Posts.Where(x => x.ID == id).FirstOrDefaultAsync();
            if(post!=null)
            {
                var serializedPost=JsonConvert.SerializeObject(post);
                await redis.StringSetAsync(key, serializedPost);
            }
            return post;
        }

        public async Task<Post> CreatePost(Post post)
        {
            this._db.Posts.Add(post);
            _db.SaveChanges();

            var redis = _redis.GetDatabase();

            var key = $"post:{post.ID}:postId";
            var serializedPost = JsonConvert.SerializeObject(post);
            await redis.StringSetAsync(key, serializedPost);

            var listKey = $"posts:{post.AuthorId}:authorId";
            await redis.ListRightPushAsync(listKey, JsonConvert.SerializeObject(post));

            return post;
        }
        public async Task<Post> UpdatePost(Post post)
        {
            this._db.Posts.Update(post);

            var redis = _redis.GetDatabase();

            var listKey = $"posts:{post.AuthorId}:authorId";
            var postDelete = await GetPostById(post.ID);
            await redis.ListRemoveAsync(listKey, JsonConvert.SerializeObject(postDelete));

            var key = $"post:{post.ID}:postId";
            await redis.KeyDeleteAsync(key);       

            await redis.StringSetAsync(key, JsonConvert.SerializeObject(post));
            await redis.ListRightPushAsync(listKey, JsonConvert.SerializeObject(post));

            return post;
        }

        public async Task<Post> DeletePost(Post post)
        {
            this._db.Posts.Remove(post);

            var redis = _redis.GetDatabase();

            var listKey = $"posts:{post.AuthorId}:authorId";
            await redis.ListRemoveAsync(listKey, JsonConvert.SerializeObject(post));

            var key = $"post:{post.ID}:postId";
            await redis.KeyDeleteAsync(key);

            return post;
        }
        public async Task<List<Post>> GetAllPosts()
        {
            List<Post> posts = await this._db.Posts.
                Include(p => p.Author)
                .Include(p => p.Comments)
                .ToListAsync();
            return posts;
        }
        public async Task<List<Post>> GetPostByAuthorId(int authorId)
        {
            var redis = _redis.GetDatabase();

            var listKey = $"posts:{authorId}:authorId";
            var redisValue = await redis.ListRangeAsync(listKey);
            if (redisValue.Any())
            {
                var redisPosts = redisValue.Select(item => JsonConvert.DeserializeObject<Post>(item)).ToList();
                return redisPosts;
            }

            List<Post> posts = await this._db.Posts.Where(x => x.AuthorId == authorId).ToListAsync();
            if (posts.Count > 0)
            {
                var serializedLikes = posts.Select(item => JsonConvert.SerializeObject(item)).ToArray();
                await redis.ListRightPushAsync(listKey, serializedLikes.Select(x => (RedisValue)x).ToArray());
            }
            else
            {
                posts = null;
            }

            return posts;
        }
    }
}