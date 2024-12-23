using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Models;
using TodoApi.Services;

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
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Basic Todo Web API", Version = "v1" });

    var cookieSecuritySchema = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Name = authCookieName,
        Description = "We use cookies for authentication.",

        Reference = new OpenApiReference
        {
            Id = "Cookie Authentication",
            Type = ReferenceType.SecurityScheme,
        }
    };

    option.AddSecurityDefinition(cookieSecuritySchema.Reference.Id, cookieSecuritySchema);

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            cookieSecuritySchema, Array.Empty<string>()
        }
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = authCookieName;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddScoped<TodoService>();

var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseMySql(dbConnectionString, serverVersion));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<TodoContext>(dbContextOptions => dbContextOptions.UseMySql(dbConnectionString, serverVersion));

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

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
