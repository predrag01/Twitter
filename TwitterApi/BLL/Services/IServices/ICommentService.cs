using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.IServices
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentByPostId(int postId);
        Task<Comment> CreateComment(CreateCommentDTO com);
        Task UpdateComment(CommentUpdateDTO com);
        Task DeleteComment(int comId);

    }
}
