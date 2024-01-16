using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> GetPostById(int ID);
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(Post post);
        Task<Post> DeletePost(Post post);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostByAuthorId(int authorId);
    }
}
