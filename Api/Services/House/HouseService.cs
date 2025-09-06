namespace HouseManagementApi.Services.House;

using HouseManagementApi.Data;
using HouseManagementApi.Dtos.House;
using HouseManagementApi.Entities;
using HouseManagementApi.Exceptions;
using Microsoft.EntityFrameworkCore;

public class HouseService(ApiDbContext context) : IHouseService
{
  private readonly ApiDbContext _context = context;

  public async Task<HouseDto> CreateNewHouseAsync(int creatingUserId, CreateHouseRequest req)
  {
    HashSet<int> memberUserIds = req.MemberUserIds;

    List<int> existingMemberIds = await _context.Users
        .Where(u => memberUserIds.Contains(u.Id))
        .Select(u => u.Id)
        .ToListAsync();

    List<int> missingUserIds = memberUserIds.Except(existingMemberIds).ToList();

    if (missingUserIds.Count != 0)
    {
      var missingIdsString = string.Join(", ", missingUserIds);
      throw new UserNotFoundException($"The following user IDs were not found: {missingIdsString}");
    }

    User? creatingUser = await _context.Users.FindAsync(creatingUserId);
    if (creatingUser is null)
    {
      throw new UserNotFoundException($"User with ID: {creatingUserId} not found");
    }

    // create new house
    House newHouse = new()
    {
      Nickname = req.Nickname,
      StreetName = req.StreetName,
      StreetNo = req.StreetNo,
      DutyTemplates = [],
      HouseUsers = [],
    };

    // add creatingUser as Admin
    newHouse.HouseUsers.Add(new HouseUser()
    {
      HouseId = newHouse.Id,
      House = newHouse,
      UserId = creatingUserId,
      UserRole = UserRole.Admin,
      JoinedAt = DateTime.UtcNow,
    });
    memberUserIds.Remove(creatingUserId);

    // add rest of specified users as Member
    foreach (var userId in memberUserIds)
    {
      newHouse.HouseUsers.Add(new HouseUser
      {
        HouseId = newHouse.Id,
        UserId = userId,
        UserRole = UserRole.Member,
        JoinedAt = DateTime.UtcNow,
      });
    }

    await _context.Houses.AddAsync(newHouse);
    await _context.SaveChangesAsync();
    return HouseDto.FromEntity(newHouse);
  }


  public async Task<HouseDto> GetHouseByIdAsync(int houseId)
  {
    House house = await FindHouseAsync(houseId);
    return HouseDto.FromEntity(house);
  }


  public async Task<HouseDto> UpdateHouseAsync(int houseId, UpdateHouseRequest req)
  {
    House house = await FindHouseAsync(houseId);
    if (req.Nickname is not null)
    {
      house.Nickname = req.Nickname;
    }
    if (req.StreetNo is not null)
    {
      house.StreetNo = req.StreetNo;
    }
    if (req.StreetName is not null)
    {
      house.StreetName = req.StreetName;
    }
    _context.Houses.Update(house);
    await _context.SaveChangesAsync();
    return HouseDto.FromEntity(house);
  }


  public async Task DeleteHouseByIdAsync(int houseId)
  {
    House house = await FindHouseAsync(houseId);
    _context.Houses.Remove(house);
    await _context.SaveChangesAsync();
  }

  private async Task<House> FindHouseAsync(int houseId)
  {
    House? house = await _context.Houses
      .Include(h => h.HouseUsers)
      .FirstOrDefaultAsync(h => h.Id == houseId);

    if (house is null)
    {
      throw new HouseNotFoundException($"House with ID: {houseId} not found");
    }
    return house;
  }
}
