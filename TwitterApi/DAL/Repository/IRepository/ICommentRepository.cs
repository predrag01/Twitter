using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetCommentById(int id);
        Task<Comment> CreateComment(Comment com);
        Task<Comment> UpdateComment(Comment com);
        Task<Comment> DeleteComment(Comment com);
        Task<List<Comment>> GetAllCommentsByPostId(int postId);
    }
}
