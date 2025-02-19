using Identity.Domain.Services;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Identity.Requests;
using Infrastructure.Models;
using Infrastructure.Models.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IBusControl busControl, IAuthService service) : ControllerBase
{
    private Uri _rabbitMqIdentityUri = new Uri($"queue:{RabbitQueueNames.IDENTITY}");
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegister request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<UserRegister, UserModel>(request, busControl, _rabbitMqIdentityUri);
        
        return Ok(ApiResult<UserModel>.Success200(res));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<UserLogin, UserModel>(request, busControl, _rabbitMqIdentityUri);
        
        return Ok(ApiResult<UserModel>.Success200(res));
    }

    [HttpPut("set-role")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> SetUserRole([FromBody] SetUserRoleRequest request)
    {
        var res = await RabbitWorker.GetRabbitMessageResponse<SetUserRoleRequest, UserModel>
        (
            request, busControl, _rabbitMqIdentityUri
        );
        
        return Ok(ApiResult<UserModel>.Success200(res));
    }
}