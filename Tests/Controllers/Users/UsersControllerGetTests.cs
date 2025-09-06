using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers.Users;

public class UsersControllerGetTests
{
    [Fact]
    public async Task GetUser_ReturnsStatusOk_WithUserDto()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        UserDto expectedUserResponse = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
        };

        mockUserService
            .Setup(service => service.GetUserByIdAsync(expectedUserResponse.Id))
            .ReturnsAsync(expectedUserResponse);

        IActionResult result = await usersController.GetUserById(expectedUserResponse.Id);

        var okObjResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<UserDto>(okObjResult.Value);

        Assert.Equivalent(expectedUserResponse, response);
    }

    [Fact]
    public async Task GetUser_ThrowsUserNotFound()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        int userId = 1;

        mockUserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ThrowsAsync(new UserNotFoundException());

        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await usersController.GetUserById(userId)
        );
    }
}

