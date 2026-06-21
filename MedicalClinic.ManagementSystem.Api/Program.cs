using MedicalClinic.ManagementSystem.Api.Extensions;
using MedicalClinic.ManagementSystem.Api.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi;
using Serilog;

namespace MedicalClinic.ManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtection-Keys")))
                .SetApplicationName("MedicalClinic.ManagementSystem");

            builder.Services.ConfigureSql(builder.Configuration);
            builder.Services.ConfigureControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Medical Clinic Management System API",
                    Version = "v1",
                    Description = "CRUD API for managing patients, doctors, appointments, and medical records."
                });
            });

            builder.Services.AddRepositories();
            builder.Services.AddServiceLayer();

            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureAuthorization();
            builder.Services.ConfigureHealthChecks();

            builder.Services.AddHostedService<DataSeedHostingService>();
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperProfile>());
            builder.Services.ConfigureCors(builder.Configuration);

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            app.ConfigureExceptionHandler();
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "openapi/{documentName}.json";
                });
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Medical Clinic Management System v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.MapGet("/", () => Results.Redirect("/swagger"))
               .ExcludeFromDescription();

            app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseDemoMiddleware();
            app.MapControllers();
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false
            });
            app.MapHealthChecks("/health/ready");

            app.Run();
        }
    }
}
