using FluentValidation;
using MediatR;
using SportManagementSystem.BuildingBlocks.Behaviors;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Assets.Infrastructure.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Services.Infrastructure.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;
using SportManagementSystem.Modules.Users.Infrastructure.Repositories;

namespace SportManagementSystem.Extensions;

/// <summary>
/// Расширения для настройки инфраструктурных компонентов приложения.
/// </summary>
/// <remarks>
/// Этот класс содержит методы для настройки базы данных и регистрации репозиториев и фабрик в контейнере зависимостей.
/// </remarks>
public static class InfrastructureHostExtensions
{
    /// <summary>
    /// Подключение и настройка библиотеки FluentValidation.
    /// </summary>
    /// <param name="services"></param>
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
    
    /// <summary>
    /// Добавляет инфраструктурные сервисы в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        // Users Module
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();

        // Assets Module
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        
        // Service Module
        services.AddScoped<ISportServiceRepository, SportServiceRepository>();
        services.AddScoped<IServicePriceRepository, ServicePriceRepository>();
    }
}