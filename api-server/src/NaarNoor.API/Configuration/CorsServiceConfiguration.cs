namespace NaarNoor.API.Configuration;

using Microsoft.Extensions.Configuration;

/// <summary>
/// CORS service registration - Environment-aware configuration
/// Development: AllowAnyOrigin (localhost development)
/// Production: Specific origin only (https://naar-noor.vercel.app)
/// </summary>
public static class CorsServiceConfiguration
{
    public static void AddCorsServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";
        
        services.AddCors(options =>
        {
            if (environment == "Production")
            {
                // Production: Specific origins only
                var corsOrigins = configuration["ApiConfiguration:CorsOrigins"] ?? "https://naar-noor.vercel.app";
                var origins = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(o => o.Trim())
                                        .ToArray();
                
                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
                
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            }
            else
            {
                // Development: Allow localhost and local network for development/testing
                // Includes: localhost:5000 (ng serve), localhost:4200 (ng serve default), 0.0.0.0, 127.0.0.1
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()      // Allow all origins in development
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    // Note: AllowCredentials() cannot be used with AllowAnyOrigin()
                    // Use specific origins below if credentials needed
                });
                
                // Optional: If credentials needed in development, uncomment this:
                // options.AddPolicy("DevelopmentPolicy", policy =>
                // {
                //     policy.WithOrigins("http://localhost:5000", "http://localhost:4200", "http://localhost:3000", "http://127.0.0.1:5000")
                //           .AllowAnyHeader()
                //           .AllowAnyMethod()
                //           .AllowCredentials();
                // });
            }
        });
    }
}
