﻿using DAL.DataContext;
using DAL.Models;
using DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private TwitterContext _db;
        private readonly IConnectionMultiplexer _redis;
        public UserRepository(TwitterContext db, IConnectionMultiplexer redis) : base(db)
        {
            _db = db;
            _redis = redis;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await this._db.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var redis = _redis.GetDatabase();

            var key = $"user:{username}:username";
            var redisValue = await redis.StringGetAsync(key);

            if(!redisValue.IsNull)
            {
                var userFromRedis = JsonConvert.DeserializeObject<User>(redisValue);
                return userFromRedis;
            }

            var userFromDb = await this._db.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            if(userFromDb != null)
            {
                var serializedUser = JsonConvert.SerializeObject(userFromDb);
                await redis.StringSetAsync(key, serializedUser);
            }

            return userFromDb;
        }

        public async Task<IQueryable<User>> GetUsersByUsername(string username, string ownerUsername)
        {
            var users = this._db.Users.Where(x => x.Username.Contains(username));

            users = users.Where(u => u.Username != ownerUsername);

            return users;
        }

        public async Task<User> UpdateUser(User user)
        {
            this._db.Users.Update(user);
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await this._db.Users.Where(x => x.ID == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> Create(User user)
        {
            _db.Users.Add(user);
            user.ID = _db.SaveChanges();

            var redisDatabase = _redis.GetDatabase();
            var key = $"user:{user.Username}:username";
            var serializedUser = JsonConvert.SerializeObject(user);

            await redisDatabase.StringSetAsync(key, serializedUser);

            return user;
        }
    }
}
