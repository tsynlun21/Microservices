using System.Text.Json;
using System.Threading.Channels;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middlewares;

public class LogConsumeMessageFilter<T>(ILogger<LogConsumeMessageFilter<T>> logger) : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        _ = context.SerializerContext.TryGetMessage(out T? msg);
        logger.LogInformation($"Message delivered to {context.DestinationAddress}{Environment.NewLine}Content: {JsonSerializer.Serialize(msg)}");
        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("LogConsumeMessage");
    }
}