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
    public class LikeService : ILikeService
    {
        private readonly TwitterContext _db;
        public readonly IUnitOfWork _unitOfWork;

        public LikeService(TwitterContext db, IUnitOfWork unitOfWork)
        {
            this._db = db;
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<Like>> GetLikesByPostId(int postId)
        {
            if(postId == 0)
            {
                throw new Exception("Invalid post Id!");
            }

            var likes = await this._unitOfWork.Like.GetLikesByPostId(postId);
            return likes;
        }

        public async Task<Like> Like(LikeDTO like)
        {
            if(like == null)
            {
                throw new Exception("Invalid like!");
            }

            var liked = new Like
            {
                UserId = like.UserId,
                PostId = like.PostId,
            };

            return await this._unitOfWork.Like.Like(liked);
        }

        public async Task<Like> Unlike(LikeDTO like)
        {
            if (like == null)
            {
                throw new Exception("Invalid like!");
            }

            var liked = await this._unitOfWork.Like.GetLikeByPostIdUserId(like.UserId, like.PostId);

            if (liked == null)
            {
                throw new Exception("Invalid like!");
            }

            var res = await this._unitOfWork.Like.Unlike(liked);
            await this._unitOfWork.Save();
            return res;
        }
    }
}
