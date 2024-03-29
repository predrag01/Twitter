﻿using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface IFollowingListService
    {
        Task<bool> CheckFollowing(int followingId, int follwedId);
        Task ChangeState(FollowUnfollow obj);
    }
}
