using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
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
        var res = await RabbitWorker.GetRabbitMessageResponse<GetVehiclesByShowRoomIdRequest, VehiclesResponse>
        (
            new GetVehiclesByShowRoomIdRequest() { ShowRoomId = id }, busControl, rabbitMqUri
        );

        var context = HttpContext;
        return Ok(ApiResult<VehiclesResponse>.Success200(res));
    }

    [HttpPost("vehicles-by-brand-and-model")]
    public async Task<IActionResult> GetVehiclesByBrandAndModel([FromBody] GetVehiclesByBrandAndModelRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetVehiclesByBrandAndModelRequest, VehiclesResponse>
        (
            request, busControl, rabbitMqUri
        );

        return Ok(ApiResult<VehiclesResponse>.Success200(res));
    }

    [HttpGet("extra-parts-for-model")]
    public async Task<IActionResult> GetExtraParts([FromQuery] string model)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<GetExtraPartsRequest, ExtraItemsResponse>(
            new GetExtraPartsRequest() { Model = model }, busControl, rabbitMqUri
        );

        return Ok(ApiResult<ExtraItemsResponse>.Success200(res));
    }

    [HttpPost("vehicles-in-price")]
    public async Task<IActionResult> GetVehiclesInPrice([FromBody] PriceLimitRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<PriceLimitRequest, VehicleInPriceResponse>
        (
            request, busControl, rabbitMqUri
        );

        return Ok(ApiResult<VehicleInPriceResponse>.Success200(res));
    }

    [HttpPost("add-vehicles")]
    [Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Merchant}")]
    public async Task<IActionResult> AddVehiclesToShowRoom([FromBody] AddVehiclesRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<AddVehiclesRequest, VehiclesResponse>
        (request, busControl, rabbitMqUri);
        
        return Ok(ApiResult<VehiclesResponse>.Success200(res));
    }

    [HttpPost("add-extra-items")]
    [Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Merchant}")]
    public async Task<IActionResult> AddExtraItems([FromBody] AddExtraItemsRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<AddExtraItemsRequest, ExtraItemsResponse>(request, busControl, rabbitMqUri);

        return Ok(ApiResult<ExtraItemsResponse>.Success200(res));

    }
    
    [HttpPost("buy")]
    [Authorize(Roles = $"{RoleConstants.User}")]
    public async Task<IActionResult> BuyVehicle([FromBody] BuyVehicleRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();
        var user     = HttpContext.Items["User"] as User;
        var purchase = await RabbitWorker.GetRabbitMessageResponse<BuyVehicleMessageRequest, Purchase>(new BuyVehicleMessageRequest()
        {
            User = user,
            Purchase = request.Purchase,
            ShowroomId = request.ShowroomId
        }, busControl, rabbitMqUri);
        return Ok(ApiResult<Purchase>.Success200(purchase));
    } 
    
}
