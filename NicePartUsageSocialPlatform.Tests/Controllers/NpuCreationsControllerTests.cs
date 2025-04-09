using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NicePartUsageSocialPlatform.Controllers;
using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Tests.Controllers;

public class NpuCreationsControllerTests
{
    [Fact]
    public async Task ScoreNpuCreation_UserOwnsCreation_ReturnsUnauthorized()
    {
        // Arrange
        var user = new User { Id = "user id" };
        
        var npuCreationServiceMock = new Mock<INpuCreationService>();
        npuCreationServiceMock
            .Setup(n => n.GetNpuCreationWithUserByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(new NpuCreation
            {
                User = user
            });
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(u => u.GetLoggedInUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        var npuCreationController = new NpuCreationsController(
            npuCreationServiceMock.Object, 
            userServiceMock.Object);

        // Act
        var request = new ScoreNpuCreationRequest(2, 3);
        var result = await npuCreationController.ScoreNpuCreation(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
        var unauthorizedResult = (UnauthorizedObjectResult)result;
        Assert.Equal("You cannot score your own creations", unauthorizedResult.Value);
    }
}