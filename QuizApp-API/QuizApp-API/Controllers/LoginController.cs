using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;

namespace QuizApp_API.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthorizer _authorizer;
        private readonly UserManager<IdentityUser> _userManager;
        public LoginController(IAuthorizer authorizer, UserManager<IdentityUser> userManager)
        {
            _authorizer = authorizer;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel userCredentials)
        {
            try
            {
                var token = await _authorizer.Authorize(userCredentials.Username, userCredentials.Password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized("Wrong username or password");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userCredentials)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = userCredentials.Username,
                    Email = userCredentials.Email,
                    EmailConfirmed = true // just for testing
                };

                var result = await _userManager.CreateAsync(user, userCredentials.Password);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Successful" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Jwt token authentication
            // has no default mechanisms
            // to expire token by hand
            return NotFound();            
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile() 
        {
            // get user by id
            var user = await _userManager.FindByIdAsync(User.Claims.FirstOrDefault(u=>u.Type == "UserId").Value);
            // return model
            return Ok(user.UserName);
        }
    }

    public class LoginModel
    { 
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
