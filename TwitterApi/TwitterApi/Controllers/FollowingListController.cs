using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
    [Route("FollowingList")]
    [ApiController]
    public class FollowingListController : Controller
    {
        [Route("ChangeState")]
        [HttpPost]
        public async Task<IActionResult> ChangeState([FromBody] FollowUnfollow obj)
        {
            try
            {
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
