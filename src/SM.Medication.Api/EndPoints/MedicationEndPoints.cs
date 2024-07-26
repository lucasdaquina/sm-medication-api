using System.Runtime.Versioning;
using SM.Medication.Api.Extensions;
using SM.Medication.Application.Commands;

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
            }).AddMetadata("Get List of Medications");


        app.MapPost("/medications",
            async (IMedicationHandler handler, CreateMedicationCommand request) =>
            {
                try
                {
                    request.Name.GuardString(nameof(request.Name));

                    var result = await handler.Handle(request);
                    if (!result)
                        return Results.Problem(
                            "Failed to create Medication",
                            statusCode: StatusCodes.Status500InternalServerError);

                    return Results.Created();
                }
                catch (Exception e)
                {
                    //Log specific exception
                    return Results.Problem(
                        e.Message,
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            }).AddMetadata("Create Medication");

    }

    public static RouteHandlerBuilder AddMetadata(this RouteHandlerBuilder builder, string description)
    {
        return builder.WithTags(MEDICATION_TAG)
            .WithMetadata(new SwaggerOperationAttribute(MEDICATION_TAG, description))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status201Created, "Created!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status400BadRequest, "Bad Request!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized, "You're not Authorized!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError, "Failed!"))
            .RequireAuthorization();
    }
}
