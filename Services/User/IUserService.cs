using HouseManagementApi.Dtos.User;

namespace HouseManagementApi.Services.User;

public interface IUserService
{
  Task<UserDto> CreateNewUserAsync(CreateUserRequest req);
  Task<UserDto> GetUserByIdAsync(int userId);
  Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest req);
  Task DeleteUserByIdAsync(int userId);
}

