namespace HouseManagementApi.Controllers;

using HouseManagementApi.Data;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Entities;
using HouseManagementApi.Exceptions.User;
using HouseManagementApi.Services.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApiDbContext context, ILogger logger, IUserService userService) : ControllerBase
{
    private readonly ApiDbContext _context = context;
    private readonly ILogger _logger = logger;
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateNewUser([FromBody] CreateUserRequest req)
    {
        _logger.LogInformation("Attempting to create a new user with email: {Email}", req.Email);

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
            _logger.LogError(ex, "An unexpected error occurred while creating a user with email: {Email}", req.Email);
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
            _logger.LogWarning(ex, "User with ID: {UserId} not found", userId);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while getting a user with ID: {UserID}", userId);
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
            _logger.LogInformation("Successfully deleted user with ID: {UserId}", user.Id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting user with ID: {UserId}", user.Id);
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }
}
