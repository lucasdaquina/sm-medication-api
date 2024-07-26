using Microsoft.EntityFrameworkCore;
using SM.Medication.Application.Interfaces;
using SM.Medication.Infrastructure.Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace SM.Medication.Api.EndPoints;

public static class MedicationEndPoints
{
    private const string MEDICATION_TAG = "Medication";
    public static void MapMedicationEndPoints(this WebApplication app)
    {
        app.MapGet("/medications", 
            async (IMedicationHandler handler) =>
            {
                return await handler.Handle();
            })
            .WithTags(MEDICATION_TAG)
            .WithMetadata(new SwaggerOperationAttribute(MEDICATION_TAG, "Get List of Medications"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status200OK, "Success!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized, "You're not Authorized!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError, "Failed!"))
            .RequireAuthorization(); ;
    }
}
