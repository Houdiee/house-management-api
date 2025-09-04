namespace HouseManagementApi.Controllers;

using HouseManagementApi.Dtos.House;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HousesController : ControllerBase
{
    public async Task<IActionResult> CreateNewHouse([FromBody] CreateHouseRequest req)
    {
        return Ok();
    }

    [HttpGet("{houseId:int}")]
    public async Task<IActionResult> GetHouseById(int houseId)
    {
        return Ok();
    }

    [HttpPatch("{houseId:int}")]
    public async Task<IActionResult> UpdatedHouse(int houseId, UpdateHouseRequest req)
    {
        return Ok();
    }

    [HttpDelete("{houseId:int}")]
    public async Task<IActionResult> DeleteHouseById(int houseId)
    {
        return Ok();
    }
}
