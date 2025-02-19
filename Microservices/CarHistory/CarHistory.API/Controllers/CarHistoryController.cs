using CarHistory.Domain.Services;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.CarHistory;
using Infrastructure.Masstransit.CarHistory.Requests;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHistory.API.Controllers;

[ApiController]
[Route("api/car_history")]
public class CarHistoryController(IBusControl busControl) : ControllerBase
{
    private readonly Uri _rabbitMqUri = new Uri($"queue:{RabbitQueueNames.CAR_HISTORY}");

    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> AddCarHistory(
        ICollection<Infrastructure.Models.CarHistory.CarHistory> carHistories)
    {
        var res = await RabbitWorker
            .GetRabbitMessageResponse<AddCarHistoryRequest, Infrastructure.Models.CarHistory.CarHistory[]>
            (
                new AddCarHistoryRequest()
                {
                    CarHistories = carHistories
                },
                busControl,
                _rabbitMqUri
            );
        return Ok(ApiResult<Infrastructure.Models.CarHistory.CarHistory[]>.Success200(res));
    }

    [HttpGet]
    public async Task<IActionResult> GetCarHistory([FromQuery] string vin)
    {
        var res = await RabbitWorker
            .GetRabbitMessageResponse<GetCarHistoryRequest, Infrastructure.Models.CarHistory.CarHistory>(new()
            {
                Vin = vin
            }, busControl, _rabbitMqUri);

        return Ok(ApiResult<Infrastructure.Models.CarHistory.CarHistory>.Success200(res));
    }
}