using Mapster;
using MapsterMapper;
using System.Reflection;

namespace SM.Medication.Api.Extensions;

public static class MapsterExtensions
{
    public static void SetupMapster(this WebApplicationBuilder builder)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

        // Scan the current assembly
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        // Scan assemblies of referenced projects
        var referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            typeAdapterConfig.Scan(assembly);
        }

        var mapperConfig = new Mapper(typeAdapterConfig);
        builder.Services.AddSingleton<IMapper>(mapperConfig);
    }
}
