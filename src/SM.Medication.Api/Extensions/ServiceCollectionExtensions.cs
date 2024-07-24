using Microsoft.OpenApi.Models;
using SM.Medication.Shared.Options;

namespace SM.Medication.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<MedicationOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(MedicationOptions.Name).Bind(settings);
            });
    }

    public static void SetupOpenApi(this IServiceCollection services)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JSON Web Token based security",
        };

        var securityReq = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };


        var info = new OpenApiInfo()
        {
            Version = "v1",
            Title = "SmartMed Medication API",
            Description = "Medication API service for any communication regard medicament.",
            TermsOfService = new Uri("https://www.smartmed.world"),
        };

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", info);
            c.AddSecurityDefinition("Bearer", securityScheme);
        });
    }


}
