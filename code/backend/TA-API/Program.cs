using Microsoft.EntityFrameworkCore;
using Serilog;
using TA_API.Data;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
    builder.Services.AddDbContext<AssessmentDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("AssessmentDB")));
}
var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.MapGet("/", () => "Technical Assessment API");
    app.MapGet("/lbhealth", () => "Technical Assessment API");
}
app.Run();
