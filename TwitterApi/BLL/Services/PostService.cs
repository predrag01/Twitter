using BLL.Helpers;
using BLL.Services.IServices;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PostService : IPostService
    {
        private readonly TwitterContext _db;
        public readonly IUnitOfWork _unitOfWork;

        public PostService(TwitterContext db, IUnitOfWork unitOfWork)
        {
            this._db = db;
            this._unitOfWork = unitOfWork;
        }
        public async Task<Post> CreatePost(CreatePostDTO post)
        {
            var postCreated = new Post
            {
                Content = post.Content,
                AuthorId=post.AuthorId,
                LikeCounter=0
            };
            return await this._unitOfWork.Post.CreatePost(postCreated);
        }
        public async Task UpdatePost(PostUpdateDTO post)
        {
            if (post != null)
            {
                var postFound = await this._unitOfWork.Post.GetPostById(post.Id);
                postFound.Content = post.Content;
                this._unitOfWork.Post.UpdatePost(postFound);
                await this._unitOfWork.Save();
            }
        
        }

        public async Task DeletePost(int postId)
        {
            var postFound = await this._unitOfWork.Post.GetPostById(postId);
            this._unitOfWork.Post.DeletePost(postFound);
            await this._unitOfWork.Save();
        }
        public async Task<List<Post>> AllPosts()
        {
                List<Post> allpostsFound = await this._unitOfWork.Post.GetAllPosts();
                return allpostsFound;
            
        }

        public async Task<List<Post>> GetPostByAuthorId(int authorId)
        {
            List<Post> allpostsFound = await this._unitOfWork.Post.GetPostByAuthorId(authorId);
            return allpostsFound;
        }
    }
}
