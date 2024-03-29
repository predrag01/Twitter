﻿using BLL.Helpers;
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
    public class UserService : IUserService
    {
        private readonly TwitterContext _db;
        public readonly IUnitOfWork _unitOfWork;
        private JwtService jwtService { get; set; }

        public UserService(TwitterContext db, IUnitOfWork unitOfWork)
        {
            this._db = db;
            this._unitOfWork = unitOfWork;
            this.jwtService = new JwtService();
        }

        public async Task<User> Register(UserRegisterDTO user)
        {
            if (user != null)
            {
                var userFound = await this._unitOfWork.User.GetUserByEmail(user.Email);
                if (userFound != null)
                {
                    throw new Exception("User with this email already exists.");
                }

                userFound = await this._unitOfWork.User.GetUserByUsername(user.Username);
                if (userFound != null)
                {
                    throw new Exception("User with this username already exists.");
                }

                if (user.Password != user.RepeatedPassword)
                {
                    throw new Exception("Password missmatch");
                }

                var userCreated = new User
                {   Name= user.Name,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    ProfilePicture = user.ProfilePicture
                };

                return await this._unitOfWork.User.Create(userCreated);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> Login(string email, string password)
        {
            var userFound = await this._unitOfWork.User.GetUserByEmail(email);

            if (userFound == null) throw new Exception("Invalid credential");

            if (!BCrypt.Net.BCrypt.Verify(password, userFound.Password)) throw new Exception("Invalid credential");

            var jwt = jwtService.Generate(userFound.ID);

            return jwt;
        }
        public async Task UpdateProfile(UserUpdateDTO user)
        {
            if (user != null)
            {
                var userFound = await this._unitOfWork.User.GetUserById(user.Id);
                userFound.Name = user.Name;
                userFound.LastName = user.LastName;
                userFound.Username = user.Username;
                userFound.Email = user.Email;
                userFound.ProfilePicture = user.ProfilePicture;

                if(user.ChangePass)
                {
                    if(BCrypt.Net.BCrypt.Verify(user.OldPass, userFound.Password))
                    {
                        userFound.Password = BCrypt.Net.BCrypt.HashPassword(user.NewPass);
                    }
                }

                this._unitOfWork.User.UpdateUser(userFound);
                await this._unitOfWork.Save();
            }
        }
        public async Task<User> GetUser(string jwt)
        {
            var token = jwtService.Verify(jwt);

            int userId = int.Parse(token.Issuer);

            var user = await this._unitOfWork.User.GetUserById(userId);

            return user;
        }
        public async Task<IQueryable<User>> Search(string username, string ownerUsername)
        {
            if (username == null)
            {
                throw new Exception("Type username for searching!");
            }

            if (ownerUsername == null)
            {
                throw new Exception("Missing username who searching!");
            }

            var users = await this._unitOfWork.User.GetUsersByUsername(username, ownerUsername);
            return users;
        }

        public async Task<User> GetUserById(int userId, int profileUserId)
        {
            if (userId == null)
            {
                throw new Exception("User id is not send!");
            }
            var user = await this._unitOfWork.User.GetUserById(profileUserId);

            user.FollowersCount = await this._unitOfWork.FollowingList.CountFollowers(profileUserId);
            user.FollowingCount = await this._unitOfWork.FollowingList.CountFollowings(profileUserId);

            if(userId != profileUserId)
            {
                bool check = await this._unitOfWork.FollowingList.CheckFollowing(userId, profileUserId);

                if (check)
                {
                    user.CheckFollowing = true;
                }
                else
                {
                    user.CheckFollowing = false;
                }
            }

            return user;
        }
    }
}
