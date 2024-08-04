using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using System.Security.Claims;

namespace QuizApp_API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfilesController(IUserProfilesService userProfilesService) 
        : ControllerBase
    {
        private readonly IUserProfilesService _userProfilesService = userProfilesService;


        [HttpGet("current-username")]
        public IActionResult GetCurrentUser()
        {
            var username = GetCurrentUserName();
            if (string.IsNullOrEmpty(username))
                return BadRequest("There is no username");
            else
                return Ok(new { username });
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
                var profile = await _userProfilesService.GetByUsernameAsync(username);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] UserProfileModel profile)
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
