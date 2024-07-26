using Microsoft.OpenApi.Models;

namespace SM.Medication.Api.Extensions;

public static class OpenAiExtensions
{
    public static void SetupOpenApi(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = AuthSchemeConstants.SmartMedAuthScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JSON Web Token based security"
        };

        var securityRequirement = new OpenApiSecurityRequirement();
        var secondSecurityDefinition = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = AuthSchemeConstants.SmartMedAuthScheme
            }
        };
        securityRequirement.Add(secondSecurityDefinition, []);

        var info = new OpenApiInfo()
        {
            Version = "v1",
            Title = "SmartMed Medication API",
            Description = "Medication API service for any communication regard medicament.",
            TermsOfService = new Uri("https://www.smartmed.world"),
        };


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(action =>
        {
            action.SwaggerDoc("v1", info);
            action.AddSecurityDefinition(AuthSchemeConstants.SmartMedAuthScheme, securityScheme);
            action.AddSecurityRequirement(securityRequirement);
            action.EnableAnnotations();
        });
    }
}
