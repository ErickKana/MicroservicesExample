using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Platform Service", Version = "1.0" });
//}
//);

var app = builder.Build();

PrepDb.PrepPopulation(app);

app.Run();
