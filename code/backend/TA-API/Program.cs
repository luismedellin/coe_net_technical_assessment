using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Core.Services;
using TA_API.Core.Validators;
using TA_API.Data;
using TA_API.Data.Repository;
using TA_API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FrontEndPolicy", policy =>
        {
            policy
                .WithOrigins("http://localhost:1234")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Validators
    builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();

    // Services
    builder.Services.AddScoped<IOrderService, OrderService>();

    // Repositories
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();

    app.UseCors("FrontEndPolicy");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Map endpoints
    app.MapHealthEndpoints();
    app.MapOrderEndpoints();
    app.MapProductEndpoints();
}
app.Run();
