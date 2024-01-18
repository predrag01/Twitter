using BLL.Helpers;
using BLL.Services.IServices;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
    [Route("Like")]
    [ApiController]
    public class LikeController : Controller
    {
        private readonly TwitterContext _db;
        public readonly ILikeService _likeService;

        public LikeController(TwitterContext db, ILikeService likeService)
        {
            this._db = db;
            _likeService = likeService;
        }

        [Route("GetLikesByPostId/{postId}")]
        [HttpGet]
        public async Task<IActionResult> GetLikesByPostId(int postId)
        {
            try
            {
                var result = await this._likeService.GetLikesByPostId(postId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Like")]
        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeDTO like)
        {
            try
            {
                var result = await this._likeService.Like(like);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Unlike")]
        [HttpDelete]
        public async Task<IActionResult> Unlike([FromBody] LikeDTO like)
        {
            try
            {
                var result = await this._likeService.Unlike(like);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
