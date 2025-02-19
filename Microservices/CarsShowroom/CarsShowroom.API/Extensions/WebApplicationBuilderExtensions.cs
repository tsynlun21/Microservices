using System.Text.Json.Serialization;
using CarsShowroom.API.Consumers;
using CarsShowroom.DataAccess.DataContext;
using CarsShowroom.Domain.Services;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Filters;
using Infrastructure.Masstransit;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CarsShowroom.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(
                name: "v1",
                info: new OpenApiInfo()
                {
                    Title   = "CarsShowroom API",
                    Version = "v1"
                });

            opt.AddSecurityDefinition(
                name: "Bearer",
                securityScheme: new OpenApiSecurityScheme
                {
                    In          = ParameterLocation.Header,
                    Description = "please insert JWT token",
                    Name        = "Authorization",
                    Type        = SecuritySchemeType.Http,
                    Scheme      = "Bearer",
                });
            opt.AddSecurityRequirement(
                securityRequirement: new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        return builder;
    }

    public static WebApplicationBuilder AddDataContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("ShowroomDB");

        builder.Services.AddDbContext<ShowroomDbContext>(opt => opt.UseNpgsql(connectionString));

        return builder;
    }

    public static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IShowroomService, ShowroomService>();

        return builder;
    }

    public static MvcOptions AddFilters(this MvcOptions options)
    {
        options.Filters.Add<LogEndpointExecutionFilter>();
        return options;
    }

    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<GetVehiclesByShowRoomIdConsumer>();
            x.AddConsumer<GetVehiclesByBrandAndModelConsumer>();
            x.AddConsumer<GetExtraPartsConsumer>();
            x.AddConsumer<GetVehiclesInPriceConsumer>();
            x.AddConsumer<AddVehiclesToShowroomConsumer>();
            x.AddConsumer<AddExtraItemsConsumer>();
            x.AddConsumer<BuyVehicleConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri("rabbitmq://host.docker.internal/"), hostConfigurator =>
                {
                    hostConfigurator.Username("guest");
                    hostConfigurator.Password("guest");
                });

                configurator.ReceiveEndpoint(
                    queueName: RabbitQueueNames.SHOWROOMS,
                    configureEndpoint: endpoint =>
                    {
                        endpoint.PrefetchCount = 5;
                        endpoint.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                        // showrooms
                        endpoint.Consumer<GetVehiclesByShowRoomIdConsumer>(context);
                        endpoint.Consumer<GetVehiclesByBrandAndModelConsumer>(context);
                        endpoint.Consumer<GetExtraPartsConsumer>(context);
                        endpoint.Consumer<GetVehiclesInPriceConsumer>(context);
                        endpoint.Consumer<AddVehiclesToShowroomConsumer>(context);
                        endpoint.Consumer<AddExtraItemsConsumer>(context);
                        endpoint.Consumer<BuyVehicleConsumer>(context);
                    });
                configurator.ConfigureJsonSerializerOptions(o =>
                {
                    o.ReferenceHandler = ReferenceHandler.Preserve;

                    return o;
                });
                configurator.ConfigureEndpoints(context);
            });
        });
        return builder;
    }

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtAuthentication(builder.Configuration["Authentication:SecretKey"]);
        
        return builder;
    }

}