using Microsoft.AspNetCore.Http;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;
using System.Drawing.Imaging;

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
                var profile = await Convert(entity);
        
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
                var profileModel = await Convert(profile);
                profileModel.Owner = new ProfileOwnerInfo { 
                    Id = owner.Id, 
                    Username = owner.UserName ?? "none" 
                };
                profilesInfos.Add(profileModel);
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

            try
            {
                var entity = await _profileRepository.GetByIdAsync(profile.Id);
                var profileEntity = Convert(profile);
                profileEntity.ImageBytes = entity.ImageBytes;
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

        private static UserProfile Convert(UserProfileInfo model)
        {
            return new UserProfile
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                OwnerId = model.Owner.Id,
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
                Image = entity.ImageBytes ?? []
        };
            return profile;
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

        public async Task UpdateProfilePhoto(IFormFile img, string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            string[] types = ["image/png", "image/jpeg"];
            if (!types.Contains(img.ContentType))
                throw new ArgumentException(nameof(img), "type is not supported");

            var userId = (await _userService.GetByNameAsync(username)).Id;
            var profile = await _profileRepository.GetByOwnerIdAsync(userId);

            profile.ImageBytes = await ConvertIFormFileToJpgByteArray(img);
            await _profileRepository.UpdateAsync(profile);
        }

        private static IFormFile CreateFormFileFromBytes(byte[] fileBytes)
        {
            var contentType = "image/jpeg";
            var fileName = "profile-avatar";
            var stream = new MemoryStream(fileBytes ?? []);
            var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }

        private async Task<byte[]> ConvertIFormFileToJpgByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null; // Or handle accordingly

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset the position to the beginning of the stream

                using (var image = System.Drawing.Image.FromStream(memoryStream))
                using (var jpgStream = new MemoryStream())
                {
                    image.Save(jpgStream, ImageFormat.Jpeg);
                    return jpgStream.ToArray();
                }
            }
        }

        private async Task<IFormFile> ConvertStaticImageToIFormFileAsync(string filePath)
        {
            // Ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // Read the file into a stream
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Create a FormFile instance from the stream
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "profile-avatar-placeholder", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg" // Adjust as needed based on the file type
            };

            return formFile;
        }
    }
}
