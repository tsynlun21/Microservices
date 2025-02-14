using System.Text.Json;
using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Response;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Models.Showrooms;
using Microsoft.AspNetCore.Mvc;

namespace CarsShowroom.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ShowRoomController(IShowroomService showroomService, ILogger<ShowRoomController> logger)
{
    [HttpGet("[action]/{id}")]
    public async Task<ICollection<Vehicle>> GetVehiclesByShowRoomIdAsync([FromRoute] long id)
    {
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(id)}: {id} STARTED");
        var res = await showroomService.GetAllVehiclesByShowroom(id);
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(id)}: {id} FINISHED");
        return res;
    } 
    
    [HttpPost("[action]")]
    public async Task<ICollection<Vehicle>> GetVehiclesByBrandAndModel([FromBody] BrandAndModelRequest request)
    {
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(BrandAndModelRequest)}: {JsonSerializer.Serialize(request)} STARTED");
        var res = await showroomService.GetAllVehiclesByBrandAndModel(request);
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(BrandAndModelRequest)}: {JsonSerializer.Serialize(request)} FINISHED");
        return res;
    }
    
    [HttpGet("[action]")]
    public async Task<ICollection<ExtraItem>> GetExtraParts([FromQuery] string model)
    {
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(model)}: {model} STARTED");
        var res = await showroomService.GetAllExtrasByModelName(model);
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(model)}: {model} FINISHED");
        return res;
    }
    
    [HttpPost("[action]")]
    public async Task<ICollection<VehiclesForShowroom>> GetVehiclesInPrice([FromBody] PriceLimitRequest request)
    {
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(PriceLimitRequest)}: {JsonSerializer.Serialize(request)} STARTED");
        var res = await showroomService.GetAllVehiclesInPrice(request);
        logger.LogInformation($"Method {nameof(GetVehiclesByShowRoomIdAsync)}, {nameof(PriceLimitRequest)}: {JsonSerializer.Serialize(request)} FINISHED");
        return res;
    }
}