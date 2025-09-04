namespace HouseManagementApi.Controllers;

using HouseManagementApi.Dtos.User;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;

// TODO: add logging

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateNewUser([FromBody] CreateUserRequest req)
    {
        UserDto newUserDto = await _userService.CreateNewUserAsync(req);
        return Created($"/api/users/{newUserDto.Id}", newUserDto);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        UserDto userDto = await _userService.GetUserByIdAsync(userId);
        return Ok(userDto);
    }

    [HttpPatch("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest req)
    {
        UserDto updatedUserDto = await _userService.UpdateUserAsync(userId, req);
        return Ok(updatedUserDto);
    }

    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUserById(int userId)
    {
        await _userService.DeleteUserByIdAsync(userId);
        return Ok(new { message = $"User with ID: {userId} successfully deleted" });
    }
}
