namespace HouseManagementApi.Controllers;

using EntityFramework.Exceptions.Common;
using HouseManagementApi.Data;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Entities;
using HouseManagementApi.Services.PasswordHasher;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ApiDbContext context, ILogger logger, IPasswordHasher passwordHasher) : ControllerBase
{
    private readonly ApiDbContext _context = context;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ILogger _logger = logger;

    [HttpPost]
    public async Task<IActionResult> CreateNewUser([FromBody] CreateUserRequest req)
    {
        _logger.LogInformation("Attempting to create a new user with email: {Email}", req.Email);
        User newUser = new()
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            HashedPassword = _passwordHasher.HashPassword(req.Password),
            HouseUsers = [],
            DutyInstances = [],
        };

        await _context.Users.AddAsync(newUser);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully created a new user with ID: {UserId} and email: {Email}", newUser.Id, newUser.Email);

            var response = UserDto.FromEntity(newUser);
            return Ok(response);
        }
        catch (UniqueConstraintException ex)
        {
            _logger.LogWarning(ex, "Failed to create user with email {Email}. A user with this email already exists", req.Email);
            return Conflict(new { message = $"User with {req.Email} already exists" });
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
        User? user = await _context.Users.FindAsync(userId);
        if (user is null)
        {
            return NotFound(new { message = $"User with ID {userId} not found" });
        }

        var response = UserDto.FromEntity(user);
        return Ok(response);
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
