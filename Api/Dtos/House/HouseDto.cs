using HouseManagementApi.Entities;

namespace HouseManagementApi.Dtos.House;

public class HouseDto
{
  public required int Id { get; set; }

  public required string Nickname { get; set; }
  public required string StreetNo { get; set; }
  public required string StreetName { get; set; }

  public required ICollection<HouseUser> HouseUsers { get; set; }
  public required ICollection<DutyTemplate> DutyTemplates { get; set; }
}
