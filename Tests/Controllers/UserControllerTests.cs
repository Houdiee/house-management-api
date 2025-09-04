using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task CreateNewUser_ReturnsStatusCreated_WithNewUserDto()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        CreateUserRequest request = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            Password = "ilovejohn123",
        };

        UserDto expectedResponse = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
        };

        mockUserService
            .Setup(service => service.CreateNewUserAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(expectedResponse);

        IActionResult result = await usersController.CreateNewUser(request);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equivalent(createdResult.Value, expectedResponse);
    }


    [Fact]
    public async Task CreateNewUser_WithExistingEmail_ThrowsUserAlreadyExistsException()
    {
        var mockUserService = new Mock<IUserService>();
        var usersController = new UsersController(mockUserService.Object);

        CreateUserRequest request = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            Password = "ilovejohn123",
        };

        mockUserService
            .Setup(service => service.CreateNewUserAsync(It.IsAny<CreateUserRequest>()))
            .ThrowsAsync(new UserAlreadyExistsException());

        await Assert.ThrowsAsync<UserAlreadyExistsException>(async () =>
            await usersController.CreateNewUser(request)
        );
    }


    [Fact]
    public async Task DeleteExistingUser_ReturnStatusOk()
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
    public async Task DeleteNonExistingUser_ThrowsUserNotFound()
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
