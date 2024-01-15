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
        public UnitOfWork _unitOfWork { get; set; }

        public PostService(TwitterContext db)
        {
            this._db = db;
            this._unitOfWork = new UnitOfWork(db);

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
                this._unitOfWork.Post.Update(postFound);
                await this._unitOfWork.Save();
            }
        
        }
        public async Task<List<Post>> AllPosts()
        {
                List<Post> allpostsFound = await this._unitOfWork.Post.GetAllPosts();
                return allpostsFound;
            
        }
    }
}
