using Microsoft.AspNetCore.Mvc;
using Moq;
using NicePartUsageSocialPlatform.Controllers;
using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Tests.Controllers;

public class UserControllerTest
{
    [Fact]
    public async Task CreateUser_UserWithUsernameAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(u => u.GetUserByUsernameAsync("username"))
            .ReturnsAsync(new User());

        var userController = new UserController(userServiceMock.Object);

        // Act
        var request = new CreateUserRequest("username", "password");
        var result = await userController.CreateUser(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequest = (BadRequestObjectResult)result;
        Assert.Equal("Username already exists", badRequest.Value);
    }
}