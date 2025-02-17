using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Filters;

public class LogEndpointExecutionFilter(ILogger<LogEndpointExecutionFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        var arguments = JsonSerializer.Serialize(context.ActionArguments);
        logger.LogInformation($"_______Starting endpoint execution {actionName} with arguments {arguments}");
        
        var timesStarted = Stopwatch.StartNew();
        await next();
        var elapsed = timesStarted.Elapsed;
        logger.LogInformation($"_______Completed endpoint execution {actionName} with arguments {arguments}, elapsed {elapsed}");
    }
}