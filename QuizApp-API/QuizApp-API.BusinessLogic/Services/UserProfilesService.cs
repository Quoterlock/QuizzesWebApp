using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class UserProfilesService(
        IUserProfileRepository userProfileRepository,
        IUserService userService) : IUserProfilesService
    {
        private readonly IUserProfileRepository _profileRepository = userProfileRepository;
        private readonly IUserService _userService = userService;

        public async Task CreateAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            
            var user = await _userService.GetByNameAsync(username) 
                ?? throw new Exception("User not found with username:" + username);

            var profile = new UserProfileInfo
            {
                DisplayName = user.NormalizedUserName ?? "Unknown",
                Owner = new ProfileOwnerInfo
                {
                    Id = user.Id,
                    Username = username
                }
            };

            await _profileRepository.AddAsync(Convert(profile));
        }

        public async Task<UserProfileInfo> GetByOwnerId(string ownerUserId)
        {
            if (string.IsNullOrEmpty(ownerUserId))
                throw new ArgumentNullException(nameof(ownerUserId));

            try
            {
                var user = await _userService.GetByIdAsync(ownerUserId);
                var entity = await _profileRepository.GetByOwnerIdAsync(ownerUserId);
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
        
        public async Task<List<UserProfileInfo>> GetRangeAsync(params string[] ownerIds)
        {
            ArgumentNullException.ThrowIfNull(ownerIds);

            var profiles = await _profileRepository.GetRangeByOwnerIdsAsync(ownerIds);
            var owners = await _userService.GetRangeByIdAsync(ownerIds);
            var profilesInfos = new List<UserProfileInfo>();
            foreach(var profile in profiles)
            {
                var owner = owners.FirstOrDefault(o => o.Id == profile.OwnerId)
                    ?? throw new ArgumentException($"Owner withId:{profile.OwnerId} doesn't exist");
                profilesInfos.Add(new UserProfileInfo
                {
                    Id = profile.Id,
                    DisplayName = profile.DisplayName,
                    ImageBytes = profile.ImageBytes,
                    Owner = new ProfileOwnerInfo { Id = owner.Id, Username = owner.UserName ?? "none" }
                });
            }
            return profilesInfos;
        }

        public async Task<UserProfileInfo> GetByUsernameAsync(string username)
        {
            if(string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var user = await _userService.GetByNameAsync(username) 
                ?? throw new Exception("User not found with username:" + username);
            var entity = await _profileRepository.GetByOwnerIdAsync(user.Id) 
                ?? throw new Exception("User profile not found");

            var profile = Convert(entity);
            profile.Owner.Username = username;
            return profile;
        }

        public async Task UpdateAsync(UserProfileInfo profile)
        {
            ArgumentNullException.ThrowIfNull(profile);

            if (string.IsNullOrEmpty(profile.Id)) 
                throw new ArgumentNullException("profile.id");
            if (profile.Owner == null) 
                throw new ArgumentNullException("profile.owner");
            if (profile.Owner.Id == null) 
                throw new ArgumentNullException("profile.owner.id");

            try
            {
                await _profileRepository.Update(Convert(profile));
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(UserProfileInfo profile)
        {
            throw new NotImplementedException();
        }

        private static UserProfile Convert(UserProfileInfo model)
        {
            return new UserProfile
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                OwnerId = model.Owner.Id,
            };
        }

        private static UserProfileInfo Convert(UserProfile entity)
        {
            return new UserProfileInfo
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
                var user = await _userService.GetByNameAsync(username);
                if (user != null)
                    return await _profileRepository.IsExistsAsync(user.Id);
                else
                    return false;
            } 
            catch (Exception)
            {
                return false;
            }
        }
    }
}
