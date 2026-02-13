using Hangfire;
using SportManagementSystem.Infrastructure.Hangfire;
using SportManagementSystem.Modules.Scheduling.Application.Dispatchers;
using SportManagementSystem.Modules.Users.Domain.Services;
using SportManagementSystem.Modules.Users.Infrastructure.Services;

namespace SportManagementSystem.Extensions;

/// <summary>
/// Расширения для настройки сервисов приложения.
/// </summary>
/// <remarks>
/// Этот класс содержит методы для добавления сервисов приложения в контейнер зависимостей.
/// </remarks>
public static class ApplicationHostExtensions
{
    /// <summary>
    /// Добавляет сервисы приложения в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<TrainingSessionStatusDispatcher>();
    }

    public static IEndpointRouteBuilder MapHangfireDashboardWithAuth(this IEndpointRouteBuilder app)
    {
        app.MapHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
        });
        
        return app;
    }
}
