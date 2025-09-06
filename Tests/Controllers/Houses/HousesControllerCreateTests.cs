using HouseManagementApi.Controllers;
using HouseManagementApi.Dtos.House;
using HouseManagementApi.Dtos.HouseUser;
using HouseManagementApi.Services.House;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers.Houses;

public class HouseControllerCreateTests
{
  public async Task CreateNewHouse_ReturnsStatusCreated_WithNewUserDto()
  {
    var mockHouseService = new Mock<IHouseService>();
    var housesController = new HousesController(mockHouseService.Object);

    var creatingUserId = 1;
    var request = new CreateHouseRequest
    {
      Nickname = "Dershane",
      StreetNo = "125",
      StreetName = "Ash Road",
      MemberUserIds = [1, 2, 3],
    };

    var expectedHouseDto = new HouseDto
    {
      Id = 1,
      Nickname = "Dershane",
      StreetNo = "125",
      StreetName = "Ash Road",
      Users = request.MemberUserIds
        .Select(id => new HouseUserDto { UserId = id })
        .ToList(),
      DutyTemplates = [],

    };

    mockHouseService
        .Setup(s => s.CreateNewHouseAsync(creatingUserId, request))
        .ReturnsAsync(expectedHouseDto);

    // Act
    // Call the controller's method with the arranged data
    var result = await housesController.CreateNewHouse(creatingUserId, request);

    // Assert
    // Verify that the result is an OkObjectResult
    var okResult = Assert.IsType<OkObjectResult>(result);

    // Verify that the value within the OkObjectResult is a HouseDto
    var returnedDto = Assert.IsType<HouseDto>(okResult.Value);
    Assert.Equal(expectedHouseDto, returnedDto);
  }
}
