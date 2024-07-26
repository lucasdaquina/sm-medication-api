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
                try
                {
                    var result = await handler.Handle();

                    return Results.Ok(result);
                }
                catch (Exception e)
                {
                    //Log specific exception
                    return Results.Problem(
                        e.Message,
                        statusCode: StatusCodes.Status500InternalServerError);
                }

            })
            .AddMetadata("Get List of Medications")
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status200OK, "Success!"));


        app.MapPost("/medications",
            async (IMedicationHandler handler, CreateMedicationCommand command) =>
            {
                try
                {
                    command.Name.GuardString(nameof(command.Name));

                    var result = await handler.Handle(command);
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
            })
            .AddMetadata("Create Medication")
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status201Created, "Created!"));

        app.MapDelete("/medications/{medicationName}",
            async (string medicationName, IMedicationHandler handler) =>
            {
                try
                {
                    medicationName.GuardString(nameof(medicationName));

                    var command = new DeleteMedicationCommand { Name = medicationName };

                    var result = await handler.Handle(command);
                    if (!result)
                        return Results.Problem(
                            "Failed to delete Medication",
                            statusCode: StatusCodes.Status500InternalServerError);

                    return Results.NoContent();
                }
                catch (Exception e)
                {
                    //Log specific exception
                    return Results.Problem(
                        e.Message,
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .AddMetadata("Delete Medication")
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status204NoContent, "Deleted!")); ;
    }

    public static RouteHandlerBuilder AddMetadata(this RouteHandlerBuilder builder, string description)
    {
        return builder.WithTags(MEDICATION_TAG)
            .WithMetadata(new SwaggerOperationAttribute(MEDICATION_TAG, description))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status400BadRequest, "Bad Request!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized, "You're not Authorized!"))
            .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError, "Failed!"))
            .RequireAuthorization();
    }
}
