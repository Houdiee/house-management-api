namespace HouseManagementApi.Controllers;

using HouseManagementApi.Dtos.House;
using HouseManagementApi.Services.House;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HousesController(IHouseService houseService) : ControllerBase
{
    private readonly IHouseService _houseService = houseService;

    // remove this later
    [HttpPost("{creatingUserId:int}")]
    public async Task<IActionResult> CreateNewHouse(int creatingUserId, [FromBody] CreateHouseRequest req)
    {
        // Get the user ID from a secure, authenticated source
        // For example, from a JWT claim:
        // var creatingUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        // remove it from the URL route
        HouseDto newHouseDto = await _houseService.CreateNewHouseAsync(creatingUserId, req);
        return Ok(newHouseDto);
    }

    [HttpGet("{houseId:int}")]
    public async Task<IActionResult> GetHouseById(int houseId)
    {
        HouseDto houseDto = await _houseService.GetHouseByIdAsync(houseId);
        return Ok(houseDto);
    }

    [HttpPatch("{houseId:int}")]
    public async Task<IActionResult> UpdatedHouse(int houseId, [FromBody] UpdateHouseRequest req)
    {
        HouseDto updatedHouseDto = await _houseService.UpdateHouseAsync(houseId, req);
        return Ok(updatedHouseDto);
    }

    [HttpDelete("{houseId:int}")]
    public async Task<IActionResult> DeleteHouseById(int houseId)
    {
        await _houseService.DeleteHouseByIdAsync(houseId);
        return Ok(new { message = $"House with ID: {houseId} successfully deleted" });
    }
}
