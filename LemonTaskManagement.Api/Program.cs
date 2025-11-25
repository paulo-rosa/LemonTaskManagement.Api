using LemonTaskManagement.Api.Configurations;

namespace LemonTaskManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.ConfigureInjector(builder.Configuration, builder.Environment);
        builder.Services.ConfigureCors(builder.Configuration, builder.Environment);
        builder.Services.ConfigureAuthentication(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddOpenApiDocument(options =>
        {
            options.Title = "LemonTaskManagement API";
            options.Description = "API for LemonTaskManagement services";
            options.Version = "1.0.0";

            options.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter your JWT token in the text input below.\n\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
            });

            options.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseOpenApi();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseCorsConfiguration(app.Environment);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseDatabase(app.Configuration);

        app.Run();
    }
}
