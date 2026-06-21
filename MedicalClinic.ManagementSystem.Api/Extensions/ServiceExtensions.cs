using MedicalClinic.ManagementSystem.Infrastructure.Repositories;
using MedicalClinic.ManagementSystem.Presentation;
using MedicalClinic.ManagementSystem.Shared.Validation;
using MedicalClinic.ManagementSystem.Services;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalClinic.ManagementSystem.Service.Contracts;

namespace MedicalClinic.ManagementSystem.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("Cors:AllowedOrigins configuration is required.");

        if (allowedOrigins.Length == 0)
        {
            throw new InvalidOperationException("Cors:AllowedOrigins must contain at least one origin.");
        }

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("X-Pagination");
            });
        });
    }

    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            opt.ReturnHttpNotAcceptable = true;
        })
                .AddNewtonsoftJson()
                .AddApplicationPart(typeof(AssemblyReference).Assembly);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressMapClientErrors = false;
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserRegistrationDtoValidator>();
    }

    public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("ApplicationDbContext")
                ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
        });
    }

    public static void ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>("database");
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped(provider => new Lazy<IPatientRepository>(() => provider.GetRequiredService<IPatientRepository>()));
        services.AddScoped(provider => new Lazy<IDoctorRepository>(() => provider.GetRequiredService<IDoctorRepository>()));
        services.AddScoped(provider => new Lazy<IAppointmentRepository>(() => provider.GetRequiredService<IAppointmentRepository>()));
        services.AddScoped(provider => new Lazy<IMedicalRecordRepository>(() => provider.GetRequiredService<IMedicalRecordRepository>()));
    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped(provider => new Lazy<IPatientService>(() => provider.GetRequiredService<IPatientService>()));
        services.AddScoped(provider => new Lazy<IDoctorService>(() => provider.GetRequiredService<IDoctorService>()));
        services.AddScoped(provider => new Lazy<IAppointmentService>(() => provider.GetRequiredService<IAppointmentService>()));
        services.AddScoped(provider => new Lazy<IMedicalRecordService>(() => provider.GetRequiredService<IMedicalRecordService>()));
        services.AddScoped(provider => new Lazy<IAuthService>(() => provider.GetRequiredService<IAuthService>()));
    }
}
