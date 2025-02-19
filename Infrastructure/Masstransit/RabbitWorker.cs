using MassTransit;

namespace Infrastructure.Masstransit;

public static class RabbitWorker
{
    static TimeSpan timeout = TimeSpan.FromSeconds(2);

    public static async Task<TOut> GetRabbitMessageResponse<TIn, TOut>(TIn request, IBusControl busControl,
        Uri rabbitQueueName, CancellationToken cancellationToken = default)
        where TIn : class
        where TOut : class
    {
#if DEBUG
        timeout = TimeSpan.FromHours(1);
#endif

        var client   = busControl.CreateRequestClient<TIn>(rabbitQueueName, timeout: timeout);
        var response = await client.GetResponse<TOut>(request, cancellationToken);

        return (response.Message as TOut)!;
    }
}