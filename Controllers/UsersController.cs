namespace HouseManagementApi.Controllers;

using HouseManagementApi.Data;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Entities;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;

// TODO: add logging

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApiDbContext context, IUserService userService) : ControllerBase
{
    private readonly ApiDbContext _context = context;
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

    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUserById(int userId)
    {
        User? user = await _context.Users.FindAsync(userId);
        if (user is null)
        {
            return NotFound(new { message = $"User with ID {userId} not found" });
        }

        _context.Users.Remove(user);

        var response = UserDto.FromEntity(user);
        try
        {
            await _context.SaveChangesAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }
}
