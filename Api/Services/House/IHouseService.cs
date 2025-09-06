using HouseManagementApi.Dtos.House;

namespace HouseManagementApi.Services.House;

public interface IHouseService
{
  Task<HouseDto> CreateNewHouseAsync(CreateHouseRequest req);
  Task<HouseDto> GetHouseByIdAsync(int houseId);
  Task<HouseDto> UpdateHouseAsync(int houseId, UpdateHouseRequest req);
  Task DeleteHouseByIdAsync(int houseId);
}
