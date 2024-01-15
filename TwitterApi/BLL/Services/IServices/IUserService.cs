﻿using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IUserService
    {
        Task<User> Register(UserRegisterDTO user);
        Task<string> Login(string email, string password);
        Task UpdateProfile(UserUpdateDTO user);

        Task<User> GetUser(string jwt);
        Task<IQueryable<User>> Search(string username, string ownerUsername);
        Task<User> GetUserById(int id, int searchUserId);
    }
}
