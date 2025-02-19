using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Services;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarsShowroom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowRoomController
    (IBusControl busControl) : ControllerBase
{
    private Uri rabbitMqUri = new Uri($"queue:{RabbitQueueNames.SHOWROOMS}");

    [HttpGet("vehicles-by-showroom/{id}")]
    public async Task<IActionResult> GetVehiclesByShowRoomId([FromRoute] long id)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetVehiclesByShowRoomIdRequest, Vehicle[]>
        (
            new GetVehiclesByShowRoomIdRequest() { ShowRoomId = id }, busControl, rabbitMqUri
        );

        var context = HttpContext;
        return Ok(ApiResult<Vehicle[]>.Success200(res));
    }

    [HttpPost("vehicles-by-brand-and-model")]
    public async Task<IActionResult> GetVehiclesByBrandAndModel([FromBody] GetVehiclesByBrandAndModelRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetVehiclesByBrandAndModelRequest, Vehicle[]>
        (
            request, busControl, rabbitMqUri
        );

        return Ok(ApiResult<Vehicle[]>.Success200(res));
    }

    [HttpGet("extra-parts-for-model")] 
    public async Task<IActionResult> GetExtraParts([FromQuery] string model)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetExtraPartsRequest, ExtraPart[]>(
            new GetExtraPartsRequest() { Model = model }, busControl, rabbitMqUri
        );

        return Ok(ApiResult<ExtraPart[]>.Success200(res));
    }

    [HttpPost("vehicles-in-price")]
    public async Task<IActionResult> GetVehiclesInPrice([FromBody] PriceLimitRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<PriceLimitRequest, VehiclesInPriceForShowroom[]>
        (
            request, busControl, rabbitMqUri
        );
        
        return Ok(ApiResult<VehiclesInPriceForShowroom[]>.Success200(res));
    }

    [HttpPost("add-vehicles")]
    [Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Merchant}")]
    public async Task<IActionResult> AddVehiclesToShowRoom([FromBody] AddVehiclesRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<AddVehiclesRequest, Vehicle[]>
        (request, busControl, rabbitMqUri);
        
        return Ok(ApiResult<Vehicle[]>.Success200(res));
    }

    [HttpPost("add-extra-parts")]
    [Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Merchant}")]
    public async Task<IActionResult> AddExtraParts([FromBody] AddExtraPartsRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<AddExtraPartsRequest, ExtraPart[]>(request, busControl, rabbitMqUri);

        return Ok(ApiResult<ExtraPart[]>.Success200(res));

    }
    
    [HttpPost("buy")]
    [Authorize(Roles = $"{RoleConstants.User}")]
    public async Task<IActionResult> BuyVehicle([FromBody] BuyVehicleRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();
        var user     = HttpContext.User.Identity!.Name;
        var purchase = await RabbitWorker.GetRabbitMessageResponse<BuyVehicleMessageRequest, Purchase>(new BuyVehicleMessageRequest()
        {
            User = new UserModel()
            {
                UserName = user,
            },
            Purchase = request.Purchase,
            ShowroomId = request.ShowroomId
        }, busControl, rabbitMqUri);
        return Ok(ApiResult<Purchase>.Success200(purchase));
    } 
    
}
