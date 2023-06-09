using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer DB");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> Using InMem DB");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InMem"));
}


builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddSingleton<IMessageBugClient, MessageBusClient>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();



//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Platform Service", Version = "1.0" });
//}
//);

var app = builder.Build();

Console.WriteLine($"--> Configuration Service endpoint: {builder.Configuration["CommandService"]}");

app.UseRouting();

app.UseEndpoints(endpoints =>
    endpoints.MapControllers()
);

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.Run();
