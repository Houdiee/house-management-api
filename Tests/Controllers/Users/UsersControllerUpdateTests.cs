using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

// TODO FIX the Assert.Equals and understnad wtf is going on
namespace Tests.Controllers.Users;

public class UsersControllerUpdateTests
{
    [Fact]
    public async Task UpdateUser_ReturnsStatusOk_WithUpdatedUserDto()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        int userId = 1;
        UpdateUserRequest updateUserRequest = new()
        {
            FirstName = "Anne",
            LastName = "Smith",
            Email = "updated@email.com",
            Password = "updatedpass123",
        };

        UserDto expectedUser = new()
        {
            Id = userId,
            FirstName = "Anne",
            LastName = "Smith",
            Email = "updated@email.com",
        };

        mockUserService
            .Setup(setup => setup.UpdateUserAsync(userId, updateUserRequest))
            .ReturnsAsync(expectedUser);

        IActionResult result = await usersController.UpdateUser(userId, updateUserRequest);

        OkObjectResult okObjResult = Assert.IsType<OkObjectResult>(result);
        UserDto returnedUser = Assert.IsType<UserDto>(okObjResult.Value);

        Assert.Equal(expectedUser.FirstName, returnedUser.FirstName);
        Assert.Equal(expectedUser.LastName, returnedUser.LastName);
        Assert.Equal(expectedUser.Email, returnedUser.Email);
    }

    [Fact]
    public async Task UpdateUser_ThrowsUserNotFound()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        int userId = 1;
        UpdateUserRequest updateUserRequest = new()
        {
            FirstName = "Anne",
            LastName = "Smith",
            Email = "updated@email.com",
            Password = "updated@password.com",
        };

        mockUserService
            .Setup(setup => setup.UpdateUserAsync(userId, updateUserRequest))
            .ThrowsAsync(new UserNotFoundException());

        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await usersController.UpdateUser(userId, updateUserRequest)
        );
    }
}
