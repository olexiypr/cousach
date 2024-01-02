using Microsoft.AspNetCore.Diagnostics;
using SQLProvider.Api.Middlewares;
using SQLProvider.Application.Setup;
using SQLProvider.Data;
using SQLProvider.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DatabaseSetup>();
builder.Services.AddTransient<IDbContext, DbContext>();
new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
new DependenciesRegistrator(builder.Services).Register();
var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var setup = scope.ServiceProvider.GetService<DatabaseSetup>();
    await setup.Setup();
}
catch (Exception e)
{
    if (!e.Message.StartsWith("42P07") || !e.Message.StartsWith("42P04"))
    {
        throw;
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionsConverterMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();