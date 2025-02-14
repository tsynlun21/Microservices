using CarsShowroom.API.Extensions;
using Infrastructure.Middlewares;

namespace CarsShowroom.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        //builder.Services.AddAuthorization();
        builder
            .AddSwagger()
            .AddDataContext()
            .AddDomainServices();
        
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        
        app.MapControllers();

        app.Run();
    }
}