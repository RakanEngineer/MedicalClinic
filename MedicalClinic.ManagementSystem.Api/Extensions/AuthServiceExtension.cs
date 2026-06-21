using MedicalClinic.ManagementSystem.Domain.Models.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using System.Security.Claims;
using System.Text;

namespace MedicalClinic.ManagementSystem.Api.Extensions;

public static class AuthServiceExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration
                         .GetSection(JwtSettings.Section)
                         .Get<JwtSettings>()
                         ?? throw new InvalidOperationException("JwtSettings section is missing or invalid.");

        services.AddOptions<JwtSettings>()
                        .Bind(configuration.GetSection(JwtSettings.Section))
                        .Validate(config => !string.IsNullOrWhiteSpace(config.SecretKey), "SecretKey is required")
                        .Validate(config => config.SecretKey?.Length >= 32, "SecretKey must be at least 32 characters")
                        .ValidateDataAnnotations()
                        .ValidateOnStart();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtSettings.Issuer,
                   ValidAudience = jwtSettings.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                   ClockSkew = TimeSpan.FromMinutes(1)
               };
           });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 8;

            opt.User.RequireUniqueEmail = true;
        })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.AdminOnly, policy =>
            {
                policy.RequireRole(ClinicRoles.Admin)
                      .RequireAuthenticatedUser()
                      .RequireClaim(ClaimTypes.NameIdentifier)
                      .RequireClaim(ClaimTypes.Role);
            });

            options.AddPolicy(AuthorizationPolicies.CanWrite, policy =>
            {
                policy.RequireRole(ClinicRoles.Admin)
                      .RequireAuthenticatedUser()
                      .RequireClaim(ClaimTypes.NameIdentifier)
                      .RequireClaim(ClaimTypes.Role);
            });

            options.AddPolicy(AuthorizationPolicies.AuthenticatedUser, policy =>
            {
                policy.RequireAuthenticatedUser()
                      .RequireRole(ClinicRoles.User, ClinicRoles.Admin);
            });
        });
    }
}
