namespace HouseManagementApi.Services.User;

using EntityFramework.Exceptions.Common;
using HouseManagementApi.Data;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Entities;
using HouseManagementApi.Exceptions.User;
using HouseManagementApi.Services.PasswordHasher;

public class UserService(ApiDbContext context, ILogger logger, IPasswordHasher passwordHasher) : IUserService
{
  private readonly ApiDbContext _context = context;
  private readonly ILogger _logger = logger;
  private readonly IPasswordHasher _passwordHasher = passwordHasher;

  public async Task<UserDto> CreateNewUserAsync(CreateUserRequest req)
  {
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
      _logger.LogInformation("Successfully created new user with ID: {UserId} and email: {Email}", newUser.Id, newUser.Email);
      return UserDto.FromEntity(newUser);
    }
    catch (UniqueConstraintException ex)
    {
      _logger.LogWarning(ex, "Failed to create user. A user with the email {Email} already exists.", req.Email);
      throw new UserAlreadyExistsException($"User with {req.Email} already exists", ex);
    }
  }


  public async Task<UserDto> GetUserByIdAsync(int userId)
  {
    User? user = await _context.Users.FindAsync(userId);
    if (user is null)
    {
      throw new UserNotFoundException($"User with ID: {userId} not found");
    }
    _logger.LogInformation("Successfully retrieved user with ID: {UserId}", user.Id);
    return UserDto.FromEntity(user);
  }


  public async Task DeleteUserByIdAsync(int userId)
  {
    User? user = await _context.Users.FindAsync(userId);
    if (user is null)
    {
      throw new UserNotFoundException($"User with ID: {userId} not found");
    }
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    _logger.LogInformation("Successfully deleted user with ID: {UserId}", user.Id);
  }
}
