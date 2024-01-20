using DAL.Repository.IRepository;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DataContext;
using StackExchange.Redis;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TwitterContext _context;
        private readonly IConnectionMultiplexer _redis;
        public ICommentRepository Comment { get; private set; }

        public ILikeRepository Like { get; private set; }

        public IPostRepository Post { get; private set; }

        public IUserRepository User { get; private set; }
        public IFollowingListRepository FollowingList { get; private set; }

        public UnitOfWork(TwitterContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
            Comment = new CommentRepository(_context, _redis);
            Like = new LikeRepository(_context, _redis);
            Post = new PostRepository(_context, _redis);
            User = new UserRepository(_context, _redis);
            FollowingList = new FollowingListRepository(_context, _redis);
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
