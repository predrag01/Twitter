using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface IFollowingListRepository : IRepository<FollowingList>
    {
        Task<bool> CheckFollowing(int followingId, int follwedId);
        Task<FollowingList> Follow(FollowingList obj);
        Task<FollowingList> Unfollow(FollowingList obj);
        Task<int> CountFollowings(int userId);
        Task<int> CountFollowers(int userId);
        Task<FollowingList> GetFollowingListbyId(int followedId, int followerId);
    }
}
