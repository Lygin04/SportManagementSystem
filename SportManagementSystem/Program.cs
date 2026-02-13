using Hangfire;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Extensions;
using SportManagementSystem.Middleware;
using SportManagementSystem.Modules.Scheduling.Application.Dispatchers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("Postgres")
        )
    );

builder.Services.AddHangfireWithPostgres(configuration);
builder.Services.AddMinio(configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddFluentValidation();
builder.Services.AddJwtAuth(configuration);
builder.Services.AddAuthModule();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

app.UseCors(cors =>
{
    cors.AllowAnyHeader();
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI();
app.MapHangfireDashboardWithAuth();

RecurringJob.AddOrUpdate<TrainingSessionStatusDispatcher>(
    "training-session-status-dispatcher",
    dispatcher => dispatcher.DispatchAsync(),
    "*/5 * * * * *");

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
