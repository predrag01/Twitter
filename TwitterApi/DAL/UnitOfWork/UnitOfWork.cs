using DAL.Repository.IRepository;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DataContext;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TwitterContext _context;

        public ICommentRepository Comment { get; private set; }

        public ILikeRepository Like { get; private set; }

        public IPostRepository Post { get; private set; }

        public IUserRepository User { get; private set; }
        public IFollowingListRepository FollowingList { get; private set; }

        public UnitOfWork(TwitterContext context)
        {
            _context = context;
            Comment = new CommentRepository(_context);
            Like = new LikeRepository(_context);
            Post = new PostRepository(_context);
            User = new UserRepository(_context);
            FollowingList = new FollowingListRepository(_context);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
