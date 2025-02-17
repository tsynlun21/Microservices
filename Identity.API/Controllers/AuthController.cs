using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Identity.Requests;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IBusControl busControl) : ControllerBase
{
    private Uri _rabbitMqIdentityUri = new Uri($"queue:{RabbitQueueNames.IDENTITY}");

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] UserRegister request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<UserRegister, UserResponse>(request, busControl, _rabbitMqIdentityUri);

        return Ok(ApiResult<UserResponse>.Success200(res));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] UserLogin request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<UserLogin, UserResponse>(request, busControl, _rabbitMqIdentityUri);
        
        return Ok(ApiResult<UserResponse>.Success200(res));
    }

    [HttpPut("set-role")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> SetUserRole([FromQuery] int userId, [FromQuery] string role)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<SetUserRoleRequest, BaseMasstransitResponse>
        (
            new SetUserRoleRequest(){UserId = userId, Role = role}, busControl, _rabbitMqIdentityUri
        );
        
        return Ok(ApiResult<BaseMasstransitResponse>.Success200(res));
    }
}