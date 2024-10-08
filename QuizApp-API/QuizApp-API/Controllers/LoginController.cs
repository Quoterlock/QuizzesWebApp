﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;

namespace QuizApp_API.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class LoginController(IUserService userManager) 
        : ControllerBase
    {
        private readonly IUserService _userManager = userManager;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel userCredentials)
        {
            try
            {
                var token = await _userManager.Authorize(userCredentials.Email, userCredentials.Password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userCredentials)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.Register(userCredentials.Username, userCredentials.Email, userCredentials.Password);
                    return Ok(new { Message = "Successful" });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else return BadRequest("Model is invalid");
        }
    }

    public class LoginModel
    { 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
