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
    public class CommentService : ICommentService
    {
        private readonly TwitterContext _db;
        public readonly IUnitOfWork _unitOfWork;

        public CommentService(TwitterContext db, IUnitOfWork unitOfWork)
        {
            this._db = db;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Comment> CreateComment(CreateCommentDTO com)
        {
            var commCreated = new Comment
            {
                CommentContent = com.CommentContent,
                UserId = com.UserId,
                PostId=com.PostId
            };
            return await this._unitOfWork.Comment.CreateComment(commCreated);
        }
        public async Task UpdateComment(CommentUpdateDTO com)
        {
            if (com != null)
            {
                var comFound = await this._unitOfWork.Comment.GetCommentById(com.Id);
                comFound.CommentContent = com.CommentContent;
                this._unitOfWork.Comment.UpdateComment(comFound);
                await this._unitOfWork.Save();
            }

        }

        public async Task DeleteComment(int comId)
        {
            var comFound = await this._unitOfWork.Comment.GetCommentById(comId);
            this._unitOfWork.Comment.DeleteComment(comFound);
            await this._unitOfWork.Save();
        }

        public async Task<List<Comment>> GetCommentByPostId(int postId)
        {
            List<Comment> allcomsFound = await this._unitOfWork.Comment.GetAllCommentsByPostId(postId);
            return allcomsFound;
        }
    }
}

