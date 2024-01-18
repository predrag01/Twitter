using DAL.DataContext;
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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private TwitterContext _db;
        public CommentRepository(TwitterContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Comment> GetCommentById(int id)
        {
            var com = await this._db.Comments.Where(x => x.ID == id).FirstOrDefaultAsync();
            return com;
        }

        public async Task<Comment> CreateComment(Comment com)
        {
            this._db.Comments.Add(com);
            com.ID = _db.SaveChanges();
            return com;
        }
        public async Task<Comment> UpdateComment(Comment com)
        {
            this._db.Comments.Update(com);
            return com;
        }

        public async Task<Comment> DeleteComment(Comment com)
        {
            this._db.Comments.Remove(com);
            return com;
        }
        public async Task<List<Comment>> GetAllCommentsByPostId(int postId)
        {
            List<Comment> coms = await this._db.Comments.Where(x => x.PostId == postId).ToListAsync();
            return coms;
        }
    }
}

