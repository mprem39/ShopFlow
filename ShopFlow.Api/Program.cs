using Microsoft.EntityFrameworkCore;
using ShopFlow.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ShopFlowDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"));

    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();


