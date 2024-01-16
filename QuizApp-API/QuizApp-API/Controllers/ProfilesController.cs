using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;

namespace QuizApp_API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IUserProfilesService _userProfilesService;

        public ProfilesController(IUserProfilesService userProfilesService)
        {
            _userProfilesService = userProfilesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? user)
        {
            string username = user ?? string.Empty;
            if(string.IsNullOrEmpty(user))
            {
                if(User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    username = User.Identity.Name;
                else 
                    throw new Exception("Current user has no username");
            }

            try
            {
                if(!await _userProfilesService.IsExists(username))
                {
                    if (!username.Equals(user))
                        await _userProfilesService.CreateAsync(username);
                    else 
                        return NotFound();
                }
                var profile = await _userProfilesService.GetByUsernameAsync(username);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
