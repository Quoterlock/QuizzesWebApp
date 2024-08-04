using Microsoft.AspNetCore.Identity;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using System.Net.WebSockets;

namespace QuizApp_API.BusinessLogic.Services
{
    public class UserProfilesService(
        IUserProfileRepository userProfileRepository,
        IUserService userService,
        IQuizzesService quizzesService
            ) : IUserProfilesService
    {
        private readonly IUserProfileRepository _profileRepository = userProfileRepository;
        private readonly IUserService _userService = userService;
        private readonly IQuizzesService _quizzesService = quizzesService;

        public async Task CreateAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            
            var user = await _userService.GetByNameAsync(username) 
                ?? throw new Exception("User not found with username:" + username);

            var profile = new UserProfileModel
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

        public async Task<UserProfileModel> GetByIdAsync(string profileId)
        {
            if (string.IsNullOrEmpty(profileId))
                throw new ArgumentNullException(nameof(profileId));

            try
            {
                var entity = await _profileRepository.GetByIdAsync(profileId);
                var user = await _userService.GetByIdAsync(entity.OwnerId);
                var profile = Convert(entity);
        
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                    profile.Owner.Username = user.UserName;
                else
                    profile.Owner.Username = "Unknown";

                profile.CreatedQuizzes = (await _quizzesService.GetAllTitlesByUserId(profileId)).ToList();
                profile.CompletedQuizzesCount = await _quizzesService.GetAllUserCompleted(user.UserName??"");
                return profile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserProfileModel> GetByUsernameAsync(string username)
        {
            if(string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var user = await _userService.GetByNameAsync(username) 
                ?? throw new Exception("User not found with username:" + username);
            var entity = await _profileRepository.GetByOwnerIdAsync(user.Id) 
                ?? throw new Exception("User profile not found");

            var profile = Convert(entity);
            profile.Owner.Username = username;
            profile.CreatedQuizzes = (await _quizzesService.GetAllTitlesByUserId(profile.Owner.Id)).ToList();
            profile.CompletedQuizzesCount = await _quizzesService.GetAllUserCompleted(username);


            return profile;
        }

        public async Task UpdateAsync(UserProfileModel profile)
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

        public async Task DeleteAsync(UserProfileModel profile)
        {
            throw new NotImplementedException();
        }

        private static UserProfile Convert(UserProfileModel model)
        {
            return new UserProfile
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                OwnerId = model.Owner.Id,
            };
        }

        private static UserProfileModel Convert(UserProfile entity)
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
