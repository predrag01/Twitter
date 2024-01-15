using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
    [Route("FollowingList")]
    [ApiController]
    public class FollowingListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
