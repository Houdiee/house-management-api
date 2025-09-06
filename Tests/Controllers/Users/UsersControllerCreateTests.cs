namespace Tests.Controllers.Users;

using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class UserControllerCreateTests()
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
        UserDto response = Assert.IsType<UserDto>(createdResult.Value);
        Assert.Equivalent(expectedResponse, response);
    }


    [Fact]
    public async Task CreateNewUser_ThrowsUserAlreadyExistsException()
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
}

