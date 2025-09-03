namespace HouseManagementApi.Services.User;

using EntityFramework.Exceptions.Common;
using HouseManagementApi.Data;
using HouseManagementApi.Dtos.User;
using HouseManagementApi.Entities;
using HouseManagementApi.Exceptions;
using HouseManagementApi.Services.PasswordHasher;

public class UserService(ApiDbContext context, IPasswordHasher passwordHasher) : IUserService
{
  private readonly ApiDbContext _context = context;
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
      return UserDto.FromEntity(newUser);
    }
    catch (UniqueConstraintException ex)
    {
      throw new UserAlreadyExistsException($"User with {req.Email} already exists", ex);
    }
  }


  public async Task<UserDto> GetUserByIdAsync(int userId)
  {
    User user = await FindUserAsync(userId);
    return UserDto.FromEntity(user);
  }


  public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest req)
  {
    User user = await FindUserAsync(userId);

    if (req.FirstName is not null)
    {
      user.FirstName = req.FirstName;
    }
    if (req.LastName is not null)
    {
      user.LastName = req.LastName;
    }
    if (req.Email is not null)
    {
      user.Email = req.Email;
    }
    if (req.Password is not null)
    {
      user.HashedPassword = _passwordHasher.HashPassword(req.Password);
    }

    _context.Users.Update(user);

    try
    {
      await _context.SaveChangesAsync();
      return UserDto.FromEntity(user);
    }
    catch (UniqueConstraintException ex)
    {
      throw new UserAlreadyExistsException($"User with {req.Email} already exists", ex);
    }
  }


  public async Task DeleteUserByIdAsync(int userId)
  {
    User user = await FindUserAsync(userId);
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
  }


  private async Task<User> FindUserAsync(int userId)
  {
    User? user = await _context.Users.FindAsync(userId);
    if (user is null)
    {
      throw new UserNotFoundException($"User with ID: {userId} not found");
    }
    return user;
  }
}
