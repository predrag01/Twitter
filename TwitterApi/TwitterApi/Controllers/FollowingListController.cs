using BLL.Services.IServices;
using BLL.Services;
using DAL.DataContext;
using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;

namespace TwitterApi.Controllers
{
    [Route("FollowingList")]
    [ApiController]
    public class FollowingListController : Controller
    {
        private readonly TwitterContext _db;
        public readonly IFollowingListService _followingListService;

        public FollowingListController(TwitterContext db, IFollowingListService followingListService)
        {
            this._db = db;
            _followingListService = followingListService;
        }

        [Route("ChangeState")]
        [HttpPost]
        public async Task<IActionResult> ChangeState([FromBody] FollowUnfollow obj)
        {
            try
            {
                await this._followingListService.ChangeState(obj);
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
