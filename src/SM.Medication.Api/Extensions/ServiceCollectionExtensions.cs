using Microsoft.OpenApi.Models;
using SM.Medication.Application.Handlers;
using SM.Medication.Application.Interfaces;
using SM.Medication.Domain.Interfaces;
using SM.Medication.Infrastructure.Services.Repositories;
using SM.Medication.Shared.Options;
using SmartMed.Medication.Auth.Constants;

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

    public static void AddInterfaces(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMedicationHandler, MedicationHandler>();
        builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
    }
}
