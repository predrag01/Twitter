using BLL.Services;
using BLL.Services.IServices;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace TwitterApi.Controllers
{
    [Route("Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly TwitterContext _db;
        public IPostService _postService { get; set; }

        public PostController(TwitterContext db)
        {
            this._db = db;
            _postService = new PostService(db);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO post)
        {
            try
            {
                var result = await this._postService.CreatePost(post);
                return Created("success", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("UpdatePost")]
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdateDTO post)
        {
            try
            {
                await this._postService.UpdatePost(post);
                return Ok(post);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DeletePost")]
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                await this._postService.DeletePost(postId);
                return Ok("Deleted Post");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("GetAllPosts")]
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            try
            {
                List<Post> posts = await this._postService.AllPosts();

                if (posts == null || posts.Count == 0)
                {
                    return NotFound(); 
                }

                return Ok(posts); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetPostByAuthorId/{authorId}")]
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPostByAuthorId(int authorId)
        {
            try
            {
                List<Post> posts = await this._postService.GetPostByAuthorId(authorId);

                //if (posts == null || posts.Count == 0)
                //{
                //    return NotFound();
                //}

                return Ok(posts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

}
