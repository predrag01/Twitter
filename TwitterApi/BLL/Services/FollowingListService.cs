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
    public class FollowingListService : IFollowingListService
    {
        private readonly TwitterContext _db;
        public readonly IUnitOfWork _unitOfWork;

        public FollowingListService(TwitterContext db, IUnitOfWork unitOfWork)
        {
            this._db = db;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckFollowing(int followingId, int follwedId)
        {
            var result = await _unitOfWork.FollowingList.CheckFollowing(followingId, follwedId);
            return result;
        }

        public async Task ChangeState(FollowUnfollow obj)
        {
            if(obj == null)
            {
                throw new Exception("Object is null!");
            }

            var following = new FollowingList
            {
                UserId = obj.FollowingId,
                FollowedId = obj.FollowedId
            };

            if (obj.Following)
            {                
                this._unitOfWork.FollowingList.Follow(following);
            }
            else
            {
                this._unitOfWork.FollowingList.Unfollow(following);
                await this._unitOfWork.Save();
            }
        }
    }
}
