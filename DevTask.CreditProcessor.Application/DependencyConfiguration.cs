namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyConfiguration).Assembly);
        });

        return services;
    }
}
