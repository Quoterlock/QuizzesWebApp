using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class UserProfilesService(
        IUserProfileRepository userProfileRepository,
        IUserService userService,
        IImagesRepository images, IImageConverter imageConverter) : IUserProfilesService
    {
        private readonly IUserProfileRepository _profileRepository = userProfileRepository;
        private readonly IUserService _userService = userService;
        private readonly IImagesRepository _images = images;
        private readonly IImageConverter _imageConverter = imageConverter;

        public async Task CreateAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (await _profileRepository.IsExistsAsync(username))
                throw new ArgumentException("Profile already exists for this user", nameof(username));

            var user = await _userService.GetByNameAsync(username)
                ?? throw new ArgumentException("User not found with username:" + username, nameof(username));

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

        public async Task<List<UserProfileInfo>> GetRangeAsync(params string[] userIds)
        {
            ArgumentNullException.ThrowIfNull(userIds);

            var profiles = await _profileRepository.GetRangeByOwnerIdsAsync(userIds);
            var owners = await _userService.GetRangeByIdAsync(userIds);
            var profilesInfos = new List<UserProfileInfo>();
            foreach (var profile in profiles)
            {
                var owner = owners.FirstOrDefault(o => o.Id == profile.OwnerId)
                    ?? throw new ArgumentException($"Owner withId:{profile.OwnerId} doesn't exist");
                var profileModel = await Convert(profile);
                profileModel.Owner = new ProfileOwnerInfo
                {
                    Id = owner.Id,
                    Username = owner.UserName ?? "none"
                };
                profilesInfos.Add(profileModel);
            }
            return profilesInfos;
        }

        public async Task<UserProfileInfo> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var user = await _userService.GetByNameAsync(username)
                ?? throw new Exception("User not found with username:" + username);
            var entity = await _profileRepository.GetByOwnerIdAsync(user.Id)
                ?? throw new Exception("User profile not found");

            var profile = await Convert(entity);
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
            if (!await _profileRepository.IsExistsAsync(profile.Owner.Id))
                throw new ArgumentException("profile doesn't exist", nameof(profile));

            try
            {
                await _profileRepository.UpdateAsync(Convert(profile));
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

        public async Task UpdateProfilePhotoAsync(byte[] image, string username)
        {
            if (image == null)
                throw new ArgumentException(nameof(image));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var profile = await GetByUsernameAsync(username);
            var imageNormalized = _imageConverter.ResizeImage(image, 200, 200);

            if (_images.IsExists(profile.ImageId))
            {
                await _images.UpdateImageAsync(profile.ImageId, imageNormalized);
            }
            else
            {
                var imageId = await _images.AddImage(imageNormalized);
                profile.ImageId = imageId;
                await UpdateAsync(profile);
            }

        }

        public async Task<byte[]> GetProfilePhotoAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var owner = await _userService.GetByNameAsync(username);
            if (owner == null)
                throw new ArgumentException($"User:{username} doesn't exist", nameof(username));
            var profile = await _profileRepository.GetByOwnerIdAsync(owner.Id);
            if (profile == null)
                throw new Exception($"Profile with owner:{owner.Id} doesn't exist");
            var imageBytes = await _images.GetImageAsync(profile.ImageId);

            return imageBytes;
        }

        public async Task<bool> IsExistsAsync(string username)
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

        private static UserProfile Convert(UserProfileInfo model)
        {
            return new UserProfile
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                OwnerId = model.Owner.Id,
                ImageId = model.ImageId
            };
        }

        private async Task<UserProfileInfo> Convert(UserProfile entity)
        {
            var profile = new UserProfileInfo
            {
                DisplayName = entity.DisplayName,
                Id = entity.Id,
                Owner = new ProfileOwnerInfo
                {
                    Id = entity.OwnerId,
                    Username = ""
                },
                ImageId = entity.ImageId
            };
            return profile;
        }
    }
}
