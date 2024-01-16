using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IPostService
    {
        Task<Post> CreatePost(CreatePostDTO post);
        Task UpdatePost(PostUpdateDTO post);
        Task DeletePost(int postId);
        Task<List<Post>> AllPosts();
        Task<List<Post>> GetPostByAuthorId(int authorId);
    }
}
