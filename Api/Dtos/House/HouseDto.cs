namespace HouseManagementApi.Dtos.House;

using HouseManagementApi.Dtos.HouseUser;
using HouseManagementApi.Entities;


public class HouseDto
{
  public required int Id { get; set; }

  public required string Nickname { get; set; }
  public required string StreetNo { get; set; }
  public required string StreetName { get; set; }

  public required ICollection<HouseUserDto> Users { get; set; }
  public required ICollection<DutyTemplate> DutyTemplates { get; set; }

  public static HouseDto FromEntity(House house)
  {
    return new HouseDto()
    {
      Id = house.Id,
      Nickname = house.Nickname,
      StreetNo = house.StreetNo,
      StreetName = house.StreetName,
      DutyTemplates = house.DutyTemplates,
      Users = house.HouseUsers.Select(HouseUserDto.FromEntity).ToList(),
    };
  }
}


