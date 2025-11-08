namespace LemonTaskManagement.Api.Configurations
{
    internal static class InjectorConfiguration
    {
        public static void ConfigureInjector(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddHttpContextAccessor();
        }

        private static void InjectorConfigurationCommands(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
        }

        private static void InjectorConfigurationQueries(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
        }
    }
}
