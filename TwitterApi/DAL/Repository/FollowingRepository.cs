using DAL.DataContext;
using DAL.DTOs;
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
    public class FollowingListRepository : Repository<FollowingList>, IFollowingListRepository
    {
        private TwitterContext _db;
        public FollowingListRepository(TwitterContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> CheckFollowing(int followingId, int followedId)
        {
            var res = await this._db.Followings.Where(x=> x.UserId == followingId && x.FollowedId == followedId).FirstOrDefaultAsync();
            if (res == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<FollowingList> Follow(FollowingList obj)
        {
            this._db.Followings.Add(obj);
            obj.ID = this._db.SaveChanges(); 
            return obj;
        }

        public async Task<FollowingList> Unfollow(FollowingList obj)
        {
            this._db.Followings.Remove(obj);
            return obj;

        }
    }
}
