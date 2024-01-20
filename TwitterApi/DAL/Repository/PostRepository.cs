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

            var key = $"post:{id}";
            var cachedPost = await redis.StringGetAsync(key);

            if (!cachedPost.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<Post>(cachedPost);
            }
            else
            {
                var post = await _db.Posts.Where(x => x.ID == id)
                    .Include(p => p.Author)
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync();

                if (post != null)
                {
                    await redis.StringSetAsync(key, JsonConvert.SerializeObject(post));
                }

                return post;
            }
        }

        public async Task<Post> CreatePost(Post post)
        {
            this._db.Posts.Add(post);
            post.ID = _db.SaveChanges();

            var redis = _redis.GetDatabase();

            var key = $"post:{post.ID}";
            await redis.StringSetAsync(key, JsonConvert.SerializeObject(post));

            return post;
        }
        public async Task<Post> UpdatePost(Post post)
        {
            this._db.Posts.Update(post);

            var redis = _redis.GetDatabase();

            var key = $"post:{post.ID}";
            await redis.StringSetAsync(key, JsonConvert.SerializeObject(post));

            return post;
        }

        public async Task<Post> DeletePost(Post post)
        {
            this._db.Posts.Remove(post);

            var redis = _redis.GetDatabase();

            var key = $"post:{post.ID}";
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
            List<Post> posts = await this._db.Posts.Where(x => x.AuthorId == authorId).ToListAsync();
            return posts;
        }
    }
}
