using Identity.Domain.Services;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Identity.Requests;
using MassTransit;

namespace Identity.API.Consumers;

public class SetUserRoleConsumer(IAuthService service) : IConsumer<SetUserRoleRequest>
{
    public async Task Consume(ConsumeContext<SetUserRoleRequest> context)
    {
        await service.SetUserRole(context.Message.UserId, context.Message.Role);

        await context.RespondAsync(new BaseMasstransitResponse()
        {
            Success = true,
            Message = "User role updated"
        });
    }
}