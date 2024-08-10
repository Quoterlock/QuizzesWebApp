using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizApp_API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfilesController(IUserProfilesService userProfilesService, IFullUserProfileService fullUserProfileService) 
        : ControllerBase
    {
        private readonly IUserProfilesService _userProfilesService = userProfilesService;
        private readonly IFullUserProfileService _fullUserProfileService = fullUserProfileService;

        [HttpGet("current-username")]
        public IActionResult GetCurrentUser()
        {
            var username = GetCurrentUserName();
            if (string.IsNullOrEmpty(username))
                return BadRequest("There is no username");
            else
                return Ok(new { username });
        }


        [HttpGet("info")]
        public async Task<IActionResult> GetCurrentUserProfileInfo()
        {
            var username = GetCurrentUserName();
            if (string.IsNullOrEmpty(username))
                return BadRequest("User id not found");
            try
            {
                var profileInfo = await _userProfilesService.GetByUsernameAsync(username);
                return Ok(profileInfo);
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? user)
        {
           string username = user ?? string.Empty;
            if(string.IsNullOrEmpty(user))
            {
                username = GetCurrentUserName();
                if(string.IsNullOrEmpty(username))
                    throw new Exception("Current user has no username");
            }

            try
            {
                if(!await _userProfilesService.IsExists(username))
                {
                    if (!username.Equals(user))
                    {
                        await _userProfilesService.CreateAsync(username);
                    }
                    else 
                        return NotFound();
                }
                var profile = await _fullUserProfileService.GetFullUserProfile(username);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-photo")]
        [Authorize]
        public async Task<IActionResult> UpdatePhoto(IFormFile image)
        {
            if (image == null)
                return BadRequest(new { message = "Image data is missing or empty." });
            try 
            {
                var username = GetCurrentUserName() ?? string.Empty;
                await _userProfilesService.UpdateProfilePhoto(image, username);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] UserProfileInfo profile)
        {
            if(profile.Owner.Username == GetCurrentUserName())
            {
                try
                {
                    await _userProfilesService.UpdateAsync(profile);
                    return Ok();
                } 
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            } return BadRequest("You are not an owner");
        }

        [HttpGet("profile-photo")]
        public async Task<IActionResult> GetPhoto(string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest();

            var profile = await _userProfilesService.GetByUsernameAsync(username);
            return Ok(File(profile.Image, "image/jpeg"));
        }

        private string GetCurrentUserName()
        {
            if (User.Claims != null && User.Claims.Any(c => c.Type == "Name"))
            {
                var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == "Name");
                if(userNameClaim != null)
                    return userNameClaim.Value;
                return string.Empty;
            }
            else return string.Empty;
        }
    }
}
