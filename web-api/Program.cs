using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Pomelo.EntityFrameworkCore.MySql;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Env.Load(".env.development");
}
else
{
    Env.Load(".env.production");
}

var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseMySql(
        dbConnectionString,
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
