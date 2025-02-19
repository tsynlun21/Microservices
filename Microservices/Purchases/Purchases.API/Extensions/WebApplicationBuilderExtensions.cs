using System.Text.Json.Serialization;
using Infrastructure.Filters;
using Infrastructure.Masstransit;
using Infrastructure.Middlewares;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Purchases.API.Consumers;
using Purchases.DataAccesss.DataContext;
using Purchases.Domain.Services;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.API.Extensions;

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
                    Title = "Purchases API",
                    Version = "v1"
                });

            opt.AddSecurityDefinition(
                name: "Bearer",
                securityScheme: new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "please insert JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
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
                                Id = "Bearer"
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
        var connectionString = builder.Configuration.GetConnectionString("PurchasesDB");
        
        builder.Services.AddDbContext<PurchasesDbContext>(opt => opt.UseNpgsql(connectionString));
        
        return builder;
    }

    public static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPurchasesService, PurchasesService>();
        
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
            
            x.AddConsumer<AddTransactionConsumer>();
            x.AddConsumer<GetTransactionConsumer>();
            x.AddConsumer<GetTransactionByIdConsumer>();
            x.AddConsumer<UpdateTransactionConsumer>();
            
            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri("rabbitmq://host.docker.internal"), hostConfigurator =>
                {
                    hostConfigurator.Username("guest");
                    hostConfigurator.Password("guest");
                });
                // configurator.Host("localhost", "/", hostConfigurator =>
                // {
                //     hostConfigurator.Username("guest");
                //     hostConfigurator.Password("guest");
                // });
                
                configurator.ReceiveEndpoint(
                    queueName: RabbitQueueNames.PURCHASES,
                    configureEndpoint: endpoint =>
                    {
                        endpoint.PrefetchCount = 5;
                        endpoint.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        endpoint.UseConsumeFilter(typeof(LogConsumeMessageFilter<>),context);
                        // purchases
                        endpoint.Consumer<AddTransactionConsumer>(context);
                        endpoint.Consumer<GetTransactionConsumer>(context);
                        endpoint.Consumer<GetTransactionByIdConsumer>(context);
                        endpoint.Consumer<UpdateTransactionConsumer>(context);
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
}