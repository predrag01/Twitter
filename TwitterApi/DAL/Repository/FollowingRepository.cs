using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class FollowingListRepository : Repository<FollowingList>, IFollowingListRepository
    {
        private TwitterContext _db;
        private readonly IConnectionMultiplexer _redis;
        public FollowingListRepository(TwitterContext db, IConnectionMultiplexer redis) : base(db)
        {
            _db = db;
            _redis = redis;
        }

        public async Task<bool> CheckFollowing(int followerId, int followedId)
        {
            var redis = _redis.GetDatabase();

            var key = $"following{followerId}";
            var following = await redis.ListRangeAsync(key);
            bool isIdInList = following.Any(element => (int)element == followedId);
            
            if(isIdInList)
            {
                return true;
            }
            else
            {
                var res = await this._db.Followings.Where(x => x.FollowerId == followerId && x.FollowedId == followedId).FirstOrDefaultAsync();

                if (res == null)
                {
                    return false;
                }
                else
                {
                    await redis.ListRightPushAsync(key, followedId.ToString());
                    return true;
                }
            }
            
        }

        public async Task<int> CountFollowers(int userId)
        {
            var redis = _redis.GetDatabase();

            var key = $"followers{userId}";
            var sizeFromRedis = await redis.ListLengthAsync(key);
            if(sizeFromRedis > 0)
            {
                return (int)sizeFromRedis;
            }

            return await this._db.Followings.Where(x => x.FollowedId == userId).CountAsync();
        }

        public async Task<int> CountFollowings(int userId)
        {
            var redis = _redis.GetDatabase();

            var key = $"following{userId}";
            var sizeFromRedis = await redis.ListLengthAsync(key);
            if (sizeFromRedis > 0)
            {
                return (int)sizeFromRedis;
            }

            return await this._db.Followings.Where(x => x.FollowerId == userId).CountAsync();
        }

        public async Task<FollowingList> Follow(FollowingList obj)
        {
            this._db.Followings.Add(obj);
            obj.ID = this._db.SaveChanges();

            var redis = _redis.GetDatabase();

            var key = $"following:{obj.FollowerId}";
            await redis.ListRightPushAsync(key, obj.FollowedId.ToString());

            var key1 = $"followers:{obj.FollowedId}";
            await redis.ListRightPushAsync(key1, obj.FollowerId.ToString());

            return obj;
        }

        public async Task<FollowingList> Unfollow(FollowingList obj)
        {
            var redis = _redis.GetDatabase();

            var key = $"following:{obj.FollowerId}";
            await redis.ListRemoveAsync(key, obj.FollowedId.ToString());

            var key1 = $"followers:{obj.FollowedId}";
            await redis.ListRemoveAsync(key1, obj.FollowerId.ToString());

            this._db.Followings.Remove(obj);
            return obj;

        }

        public async Task<FollowingList> GetFollowingListbyId(int followedId, int followerId)
        {
            return await this._db.Followings.Where(x => x.FollowedId == followedId && x.FollowerId == followerId).FirstOrDefaultAsync();
        }
    }
}
