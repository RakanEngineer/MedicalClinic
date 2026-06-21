using Microsoft.AspNetCore.Identity;
using MedicalClinic.ManagementSystem.Shared.Authorization;

namespace MedicalClinic.ManagementSystem.Api.Services;

public class DataSeedHostingService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly ILogger<DataSeedHostingService> logger;

    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (!env.IsDevelopment()) return;

        if (!configuration.GetValue("SeedData:Enabled", true))
        {
            logger.LogInformation("Data seed is disabled.");
            return;
        }

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            await CreateRolesAsync(roleManager, [ClinicRoles.Admin, ClinicRoles.User]);
            logger.LogInformation("Template roles are ready.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Template role seed failed. The API will continue to start.");
        }
    }

    private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, string[] roleNames)
    {
        foreach (string roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName)) continue;

            var result = await roleManager.CreateAsync(new IdentityRole { Name = roleName });

            if (!result.Succeeded) throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors.Select(error => error.Description)));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
