using FluentAssertions;
using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task CreateNewUser_ReturnsOkResult()
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

        result
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
