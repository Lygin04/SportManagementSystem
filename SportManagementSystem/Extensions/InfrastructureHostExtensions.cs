using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Minio;
using SportManagementSystem.BuildingBlocks.Behaviors;
using SportManagementSystem.Modules.Images.Infrastructure;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Assets.Infrastructure.Repositories;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Clients.Infrastructure.Repositories;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Infrastructure.Repositories;
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

    public static IServiceCollection AddHangfireWithPostgres(this IServiceCollection services,
        IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres")
                                       ?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(postgresConnectionString)));
        services.AddHangfireServer();

        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Minio").Get<MinioOptions>() ?? new MinioOptions();
        services.AddSingleton(options);

        var client = new MinioClient()
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(options.UseSsl)
            .Build();

        services.AddSingleton<IMinioClient>(client);

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

        // Images Module
        services.AddScoped<IImageRepository, ImageRepository>();
        
        // Service Module
        services.AddScoped<ISportServiceRepository, SportServiceRepository>();
        services.AddScoped<IServicePriceRepository, ServicePriceRepository>();

        // Clients Module
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IMembershipTemplateRepository, MembershipTemplateRepository>();

        // Scheduling Module
        services.AddScoped<IServiceScheduleRepository, ServiceScheduleRepository>();
        services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
    }
}
