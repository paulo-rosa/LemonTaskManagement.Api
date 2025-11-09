using LemonTaskManagement.Api.Configurations;

namespace LemonTaskManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.ConfigureInjector(builder.Configuration, builder.Environment);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddOpenApiDocument(options =>
        {
            options.Title = "LemonTaskManagement API";
            options.Description = "API for LemonTaskManagement services";
            options.Version = "1.0.0";
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseDatabase(app.Configuration);

        app.Run();
    }
}
