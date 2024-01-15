using DAL.DataContext;
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
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private TwitterContext _db;
        public PostRepository(TwitterContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Post> GetPostById(int id)
        {
            var post = await this._db.Posts.Where(x => x.ID == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<Post> CreatePost(Post post)
        {
            this._db.Posts.Add(post);
            post.ID = _db.SaveChanges();
            return post;
        }
        public async Task<Post> UpdatePost(Post post)
        {
            this._db.Posts.Update(post);
            return post;
        }

        public async Task<Post> DeletePost(Post post)
        {
            this._db.Posts.Remove(post);
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

    }
}
