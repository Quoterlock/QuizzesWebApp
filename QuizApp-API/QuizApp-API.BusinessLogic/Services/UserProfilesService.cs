using Microsoft.AspNetCore.Identity;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class UserProfilesService : IUserProfilesService
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public UserProfilesService(IUserProfileRepository userProfileRepository, UserManager<IdentityUser> userManager)
        { 
            _profileRepository = userProfileRepository;
            _userManager = userManager;
        }

        public async Task CreateAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var profile = new UserProfileModel
                    {
                        DisplayName = user.NormalizedUserName ?? "Unknown",
                        Owner = new ProfileOwnerInfo
                        {
                            Id = userId,
                            Username = user.UserName
                        }
                    };
                    await _profileRepository.AddAsync(Convert(profile));
                }
                else throw new Exception("User not found with id:" + userId);
            }
            else throw new ArgumentNullException(nameof(userId));
        }

        public async Task<UserProfileModel> GetByIdAsync(string profileId)
        {
            if (!string.IsNullOrEmpty(profileId))
            {
                try
                {
                    var entity = await _profileRepository.GetByIdAsync(profileId);
                    var user = await _userManager.FindByIdAsync(entity.OwnerId);
                    var profile = Convert(entity);
                    if (user != null && !string.IsNullOrEmpty(user.UserName))
                        profile.Owner.Username = user.UserName;
                    else
                        profile.Owner.Username = "Unknown";
                    return profile;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else throw new ArgumentNullException(nameof(profileId));
        }

        public async Task<UserProfileModel> GetByUsernameAsync(string username)
        {
            if(!string.IsNullOrEmpty(username))
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var entity = await _profileRepository.GetByOwnerIdAsync(user.Id);
                    if (entity != null)
                    {
                        var profile = Convert(entity);
                        profile.Owner.Username = username;
                        return profile;
                    }
                    else throw new Exception("User profile not found");
                }
                else throw new Exception("User not found with username:" + username);
            }
            else throw new ArgumentNullException(nameof(username));
        }

        public async Task UpdateAsync(UserProfileModel profile)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(UserProfileModel profile)
        {
            throw new NotImplementedException();
        }

        private UserProfile Convert(UserProfileModel model)
        {
            return new UserProfile
            {
                DisplayName = model.DisplayName,
                OwnerId = model.Owner.Id,
            };
        }

        private UserProfileModel Convert(UserProfile entity)
        {
            return new UserProfileModel
            {
                DisplayName = entity.DisplayName,
                Id = entity.Id,
                Owner = new ProfileOwnerInfo
                {
                    Id = entity.OwnerId,
                    Username = ""
                }
            };
        }

        public async Task<bool> IsExists(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                    return await _profileRepository.IsExistsAsync(user.Id);
                else
                    return false;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
