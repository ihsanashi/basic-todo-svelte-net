namespace TodoApi.Extensions
{
  public static class CookieExtension
  {
    public static IServiceCollection ConfigureAuthCookies(this IServiceCollection services, string authCookieName)
    {
      services.ConfigureApplicationCookie(options =>
      {
        options.Cookie.Name = authCookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
      });

      return services;
    }
  }
}