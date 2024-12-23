using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables based on the environment
if (builder.Environment.IsDevelopment())
{
    Env.Load(".env.development");
}
else
{
    Env.Load(".env");
}
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

const string authCookieName = "BasicTodo.API-Auth";

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(authCookieName);
builder.Services.ConfigureAuthCookies(authCookieName);

builder.Services.AddScoped<TodoService>();

var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseMySql(dbConnectionString, serverVersion));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
options.SignIn.RequireConfirmedEmail = true)
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<TodoContext>(dbContextOptions => dbContextOptions.UseMySql(dbConnectionString, serverVersion));

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();
app.MapLogoutEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapSwagger().RequireAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
