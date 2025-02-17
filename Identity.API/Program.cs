using Identity.API.Extensions;

namespace Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting(o => o.LowercaseUrls = true);
        builder
            .AddAuthorizationAndIdentity()
            .AddAuthentificationOptions()
            .AddSwagger()
            .AddDataContext()
            .AddDomainServices()
            .AddMassTransit();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Urls.Add("http://*:5001");
        app.Run();
    }
}