using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;

namespace QuizApp_API.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IAuthorizer _authorizer;
        private IUserManager _userManager;
        public LoginController(IAuthorizer authorizer, IUserManager userManager)
        {
            _authorizer = authorizer;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel userCredentials)
        {
            try
            {
                var token = _authorizer.Authorize(userCredentials.Login, userCredentials.Password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginModel userCredentials)
        {
            // check if user exists?
            // create user account
            // send Ok response
            return NotFound();
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
        public IActionResult Profile() 
        {
            // get user by id
            var user = _userManager.GetUser(u => u.Id == User.Claims.First().Value);
            // return model
            return Ok(user.Username);
        }
    }

    public class LoginModel
    { 
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
