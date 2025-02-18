using CarHistory.Domain.Services;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHistory.API.Controllers;

[ApiController]
[Route("api/car_history")]
public class CarHistoryController(ICarHistoryService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> AddCarHistory(ICollection<Infrastructure.Models.CarHistory.CarHistory> carHistories)
    {
        await service.AddCarHistoryAsync(carHistories);
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCarHistory(string vin)
    {
        var res = await service.GetCarHistoryAsync(vin); 

        return Ok(ApiResult<Infrastructure.Models.CarHistory.CarHistory>.Success200(res));
    } 
}