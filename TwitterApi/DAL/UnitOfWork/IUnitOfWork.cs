using DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICommentRepository Comment { get; }
        ILikeRepository Like { get; }
        IPostRepository Post { get; }
        IUserRepository User { get; }
        IFollowingListRepository FollowingList { get; }
        Task Save();
    }
}
