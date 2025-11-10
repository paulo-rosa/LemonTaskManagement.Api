namespace LemonTaskManagement.Api.Configurations;

public static class CorsConfiguration
{
    private const string DevelopmentPolicyName = "DevelopmentCorsPolicy";

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DevelopmentPolicyName, policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }

    public static void UseCorsConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseCors(DevelopmentPolicyName);
        }
    }
}
