﻿using BLL.Services;
using BLL.Services.IServices;
using DAL.DataContext;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TwitterContext _db;
        public readonly IUserService _userService;

        public UserController(TwitterContext db, IUserService userService)
        {
            this._db = db;
            _userService = userService;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO user)
        {
            try
            {
                var result = await this._userService.Register(user);
                return Created("success", result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
        {
            try
            {
                var result = await this._userService.Login(user.Email, user.Password);

                Response.Cookies.Append("jwt", result, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });

                return Ok(new { message = "success" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("UpdateProfile")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO user)
        {
            try
            {
                await this._userService.UpdateProfile(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var user = await this._userService.GetUser(jwt);

                return Ok(user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }
        
        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions { SameSite = SameSiteMode.None, Secure = true });

            return Ok(new { message = "success" });
        }

        [Route("Search/{username}/{ownerUsername}")]
        [HttpGet]
        public async Task<IActionResult> Search(string username, string ownerUsername)
        {
            try
            {
                var users = await this._userService.Search(username, ownerUsername);

                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Profile/{profileUserId}/{searchUserId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById(int profileUserId, int searchUserId)
        {
            try
            {
                var user = await this._userService.GetUserById(searchUserId, profileUserId);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
