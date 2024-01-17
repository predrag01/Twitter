﻿using BLL.Services.IServices;
using DAL.DataContext;
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
    }
}
