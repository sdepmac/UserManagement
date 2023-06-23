namespace UsersApi.Extensions;

using UsersApi.Repositories;
using UsersApi.Services;
using UsersApi.Wrappers;

/// <summary>
/// Inject dependencies.
/// </summary>
public static class DependencyHandler
{
    /// <summary>
    /// Inject dependencies.
    /// </summary>
    /// <param name="services">services.</param>
    /// <returns>service collection.</returns>
    public static IServiceCollection InjectDependencies(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IGuidGenerator, GuidGenerator>()
            .AddScoped<IUserRepository, UserRepository>();
    }
}
