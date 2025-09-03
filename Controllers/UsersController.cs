namespace HouseManagementApi.Controllers;

using HouseManagementApi.Dtos.User;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;

// TODO: add logging
// TODO: add global exception handler to return 500 status code

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateNewUser([FromBody] CreateUserRequest req)
    {
        try
        {
            UserDto response = await _userService.CreateNewUserAsync(req);
            return Ok(response);
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            UserDto response = await _userService.GetUserByIdAsync(userId);
            return Ok(response);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    [HttpPatch("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest req)
    {
        try
        {
            UserDto updatedUserDto = await _userService.UpdateUserAsync(userId, req);
            return Ok(updatedUserDto);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUserById(int userId)
    {
        try
        {
            await _userService.DeleteUserByIdAsync(userId);
            return Ok(new { message = $"User with ID: {userId} successfully deleted" });
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
