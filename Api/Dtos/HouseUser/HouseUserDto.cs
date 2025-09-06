namespace HouseManagementApi.Dtos.HouseUser;

using HouseManagementApi.Entities;

public class HouseUserDto
{
  public required int Id { get; set; }

  public required int UserId { get; set; }
  public required int HouseId { get; set; }

  public required UserRole UserRole { get; set; }
  public required DateTime JoinedAt { get; set; }

  public static HouseUserDto FromEntity(HouseUser houseUser)
  {
    return new HouseUserDto()
    {
      Id = houseUser.Id,
      UserId = houseUser.UserId,
      HouseId = houseUser.HouseId,
      UserRole = houseUser.UserRole,
      JoinedAt = houseUser.JoinedAt,
    };
  }
}
