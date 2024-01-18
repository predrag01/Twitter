using BLL.Services.IServices;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
    [Route("Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly TwitterContext _db;
        public readonly ICommentService _comService;

        public CommentController(TwitterContext db, ICommentService commentService)
        {
            this._db = db;
            _comService = commentService;
        }

        [Route("CreateCom")]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO com)
        {
            try
            {
                var result = await this._comService.CreateComment(com);
                return Created("success", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("UpdateComment")]
        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDTO com)
        {
            try
            {
                await this._comService.UpdateComment(com);
                return Ok(com);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DeleteComment")]
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int comId)
        {
            try
            {
                await this._comService.DeleteComment(comId);
                return Ok("Deleted Comment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Route("GetCommentByPostId/{postId}")]
        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetCommentByPostId(int postId)
        {
            try
            {
                List<Comment> coms = await this._comService.GetCommentByPostId(postId);

                return Ok(coms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

}
