using Microsoft.OpenApi.Models;

namespace TodoApi.Extensions
{
  public static class SwaggerExtension
  {
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string authCookieName)
    {
      services.AddSwaggerGen(option =>
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

      return services;
    }
  }
}