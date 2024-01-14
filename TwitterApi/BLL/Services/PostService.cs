using BLL.Services.IServices;
using DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly TwitterContext _db;
        public UnitOfWork _unitOfWork { get; set; }

        public ChatMessageService(EnaContext db)
        {
            this._db = db;
            this._unitOfWork = new UnitOfWork(db);
        }
    }
}
