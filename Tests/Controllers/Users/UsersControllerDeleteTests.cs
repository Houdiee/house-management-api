using HouseManagementApi.Controllers;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers.Users;

public class UsersControllerDeleteTests
{
    [Fact]
    public async Task DeleteUser_ReturnStatusOk()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        int userId = 1;

        mockUserService
            .Setup(service => service.DeleteUserByIdAsync(userId))
            .Returns(Task.CompletedTask);

        IActionResult result = await usersController.DeleteUserById(userId);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ThrowsUserNotFound()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        int userId = 1;

        mockUserService
            .Setup(service => service.DeleteUserByIdAsync(userId))
            .ThrowsAsync(new UserNotFoundException());

        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await usersController.DeleteUserById(userId)
        );
    }
}

