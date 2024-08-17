using Microsoft.AspNetCore.Identity;
using Moq;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace UnitTests
{
    public class UserProfilesServiceTests
    {
        [Fact]
        public async Task Get_user_profile_by_username_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object, 
                userServiceMock.Object, 
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { 
                    UserName = username, Id = ownerId 
                });
            userProfileRepoMock.Setup(m => m.GetByOwnerIdAsync(ownerId))
                .ReturnsAsync(new UserProfile { 
                    DisplayName = "UserName", 
                    Id = "1", ImageId = "1", OwnerId = ownerId
                });

            var expected = new UserProfileInfo
            {
                Id = "1",
                DisplayName = "UserName",
                ImageId = "1",
                Owner = new ProfileOwnerInfo { Id = "0", Username = username }
            };

            // Act
            var actual = await sut.GetByUsernameAsync(username);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_user_profile_by_non_existring_username_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);
            
            userProfileRepoMock.Setup(m => m.GetByOwnerIdAsync(ownerId))
                .ReturnsAsync(new UserProfile
                {
                    DisplayName = "UserName",
                    Id = "1",
                    ImageId = "1",
                    OwnerId = ownerId
                });

            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetByUsernameAsync(username));

        }

        [Fact]
        public async Task Get_non_existring_user_profile_by_username_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser
                {
                    UserName = username,
                    Id = ownerId
                });
            
            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetByUsernameAsync(username));    
        }

        [Fact]
        public async Task Get_user_profiles_info_for_multiple_users_test()
        {
            // Arrange
            
            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            string[] ids = ["1", "2", "3"];
            UserProfile[] profiles = [
                    new () { DisplayName = "name1", Id="11", ImageId="0", OwnerId = "1"},
                    new () { DisplayName = "name2", Id="22", ImageId="0", OwnerId = "2"},
                    new () { DisplayName = "name3", Id="33", ImageId="0", OwnerId = "3"}
                ];
            IdentityUser[] users = [
                    new() { Id="1", UserName="user1"},
                    new() { Id="2", UserName="user2"},
                    new() { Id="3", UserName="user3"},
                ];
            userProfileRepoMock.Setup(m => m.GetRangeByOwnerIdsAsync(ids))
                .ReturnsAsync(profiles);
            userServiceMock.Setup(m => m.GetRangeByIdAsync(ids))
                .ReturnsAsync(users);

            List<UserProfileInfo> expected = [
                    new() { Id = "11", DisplayName = "name1", ImageId="0", Owner = { Id="1", Username ="user1"} },
                    new() { Id = "22", DisplayName = "name2", ImageId="0", Owner = { Id="2", Username ="user2"} },
                    new() { Id = "33", DisplayName = "name3", ImageId="0", Owner = { Id="3", Username ="user3"} },
            ];

            // Act
            var actual = await sut.GetRangeAsync(ids);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_multiple_user_profiles_with_empty_ids_array_test()
        {
            // Arrange
            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            // Act
            var actual = await sut.GetRangeAsync();

            // Assert
            Assert.Equal([], actual);
        }

        [Fact]
        public async Task Update_profile_photo_test()
        {
            // Arrange
            // Arrange
            var username = "user";
            var ownerId = "0";
            var imageId = "image-id";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser
                {
                    UserName = username,
                    Id = ownerId
                });
            userProfileRepoMock.Setup(m => m.GetByOwnerIdAsync(ownerId))
                .ReturnsAsync(new UserProfile
                {
                    DisplayName = "UserName",
                    Id = "1",
                    ImageId = imageId,
                    OwnerId = ownerId
                });
            imagesRepoMock.Setup(m=>m.IsExists(imageId))
                .Returns(true);

            byte[] bytes = [];

            // Act
            await sut.UpdateProfilePhotoAsync(bytes, username);
            
            //Assert
            imagesRepoMock.Verify(m => m.UpdateImageAsync(imageId, bytes), Times.Once);
            imagesRepoMock.Verify(m => m.AddImage(bytes), Times.Never);
        }

        [Fact]
        public async Task Update_profile_photo_for_the_first_time_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";
            var imageId = "";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser
                {
                    UserName = username,
                    Id = ownerId
                });
            userProfileRepoMock.Setup(m => m.GetByOwnerIdAsync(ownerId))
                .ReturnsAsync(new UserProfile
                {
                    DisplayName = "UserName",
                    Id = "1",
                    ImageId = imageId,
                    OwnerId = ownerId
                });
            userProfileRepoMock.Setup(m => m.IsExistsAsync(ownerId))
                .ReturnsAsync(true);
            imagesRepoMock.Setup(m => m.IsExists(imageId))
                .Returns(false);

            byte[] bytes = [];

            // Act
            await sut.UpdateProfilePhotoAsync(bytes, username);

            //Assert
            imagesRepoMock.Verify(m => m.UpdateImageAsync(imageId, bytes), Times.Never);
            imagesRepoMock.Verify(m => m.AddImage(bytes), Times.Once);
            userProfileRepoMock.Verify(m => m.UpdateAsync(It.IsAny<UserProfile>()), Times.Once);
        }

        [Fact]
        public async Task Get_profile_photo_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";
            var imageId = "1";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { Id = ownerId });
            userProfileRepoMock.Setup(m => m.GetByOwnerIdAsync(ownerId))
                .ReturnsAsync(new UserProfile { ImageId = imageId });
            var bytes = new byte[] { 1, 2, 3 };

            imagesRepoMock.Setup(m => m.GetImageAsync(imageId))
                .ReturnsAsync(bytes);

            // Act
            var actual = await sut.GetProfilePhotoAsync(username);
            
            // Assert
            Assert.Equal(bytes, actual);
        }

        [Fact]
        public async Task Get_profile_photo_for_non_existing_profile_owner_test()
        {
            // Arrange
            var username = "user";
            
            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.GetProfilePhotoAsync(username));
        }

        [Fact]
        public async Task Get_profile_photo_for_non_existring_profile_test()
        {
            // Arrange
            var username = "user";
            var ownerId = "0";
            
            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { Id = ownerId });
            
            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.GetProfilePhotoAsync(username));
        }

        [Fact]
        public async Task Is_profile_exists_test()
        {
            // Arrange
            var username = "user1";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { Id = "1", UserName = "user1" });
            userProfileRepoMock.Setup(m => m.IsExistsAsync("1"))
                .ReturnsAsync(true);

            // Act
            var actual = await sut.IsExistsAsync(username);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public async Task Is_profile_exists_with_non_existing_username_test()
        {
            // Arrange
            var username = "user1";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            // Act
            var actual = await sut.IsExistsAsync(username);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task Is_profile_exists_with_non_existing_profile_test()
        {
            // Arrange
            var username = "user1";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { Id = "1", UserName = "user1" });

            // Act
            var actual = await sut.IsExistsAsync(username);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task Create_user_profile_test()
        {
            //Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userServiceMock.Setup(m => m.GetByNameAsync(username))
                .ReturnsAsync(new IdentityUser { Id = "1", UserName = username, NormalizedUserName="USER" });
            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(false);

            var newProfile = new UserProfile
            {
                Id = "",
                ImageId = "",
                DisplayName = "USER",
                OwnerId = "1"
            };

            // Act
            await sut.CreateAsync(username);

            // Assert
            userProfileRepoMock.Verify(m => m.AddAsync(It.Is<UserProfile>(p=>p.Equals(newProfile))), Times.Once);
        }

        [Fact]
        public async Task Create_user_profile_for_non_existing_user_test()
        {
            // Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(false);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(username));
        }

        [Fact]
        public async Task Create_user_profile_if_profile_already_exists_test()
        {
            // Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(true);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(username));
        }

        [Fact]
        public async Task Update_user_profie_test()
        {
            // Arrange
            var username = "user";
            var userId = "userid";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(userId))
                .ReturnsAsync(true);

            var profileModel = new UserProfileInfo
            {
                DisplayName = "User",
                Id = "1",
                ImageId = "0",
                Owner = new ProfileOwnerInfo { Id = userId, Username = username }
            };

            var profileEntity = new UserProfile
            {
                DisplayName = profileModel.DisplayName,
                Id = profileModel.Id,
                ImageId = profileModel.ImageId,
                OwnerId = profileModel.Owner.Id
            };

            // Act
            await sut.UpdateAsync(profileModel);

            // Assert
            userProfileRepoMock.Verify(m => m.UpdateAsync(It.Is<UserProfile>(p => p.Equals(profileEntity))), Times.Once);

        }

        [Fact]
        public async Task Update_non_existring_user_profile_test()
        {
            // Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(false);

            var profileModel = new UserProfileInfo
            {
                DisplayName = "User",
                Id = "1",
                ImageId = "0",
                Owner = new ProfileOwnerInfo { Id = "2", Username = username }
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(profileModel));
        }

        [Fact]
        public async Task Update_user_profile_with_no_id_test()
        {
            // Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(true);

            var profileModel = new UserProfileInfo
            {
                DisplayName = "User",
                Id = "",
                ImageId = "0",
                Owner = new ProfileOwnerInfo { Id = "2", Username = username }
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.UpdateAsync(profileModel));
        }

        [Fact]
        public async Task Update_user_profile_with_no_owner_test()
        {
            // Arrange
            var username = "user";

            var userProfileRepoMock = new Mock<IUserProfileRepository>();
            var userServiceMock = new Mock<IUserService>();
            var imagesRepoMock = new Mock<IImagesRepository>();

            var sut = new UserProfilesService(
                userProfileRepoMock.Object,
                userServiceMock.Object,
                imagesRepoMock.Object);

            userProfileRepoMock.Setup(m => m.IsExistsAsync(username))
                .ReturnsAsync(true);

            var profileModel = new UserProfileInfo
            {
                DisplayName = "User",
                Id = "1",
                ImageId = "0",
                Owner = new ProfileOwnerInfo { }
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(profileModel));
        }
    }
}
