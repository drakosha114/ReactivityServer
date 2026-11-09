using Application.Activities.Queries;
using Application.Core;
using Microsoft.EntityFrameworkCore;

using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMediatR(cfg => {
    cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    cfg.RegisterServicesFromAssembly(typeof(GetActivityList.Handler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetActivityById.Handler).Assembly);
});
builder.Services.AddAutoMapper(opt => { }, typeof(MappingProfiles).Assembly);


var app = builder.Build();


app.MapControllers();
app.UseCors(policy => policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader());

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try 
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();