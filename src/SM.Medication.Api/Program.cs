using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SM.Medication.Api.Extensions;
using SM.Medication.Auth;
using SM.Medication.Auth.Extensions;
using SM.Medication.Auth.Options;
using SM.Medication.Infrastructure.Persistence;
using SmartMed.Medication.Auth.Constants;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));

builder.Services.SetupOpenApi();

builder.Services.AddDbContext<SmartMedMedicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicationDb")));

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = AuthSchemeConstants.SmartMedAuthScheme)
    .AddScheme<SMAuthSchemeOptions, SmartMedAuthenticationHandler>(AuthSchemeConstants.SmartMedAuthScheme, options => { });

builder.Services.AddAuthorization();

var app = builder.Build();

//var supportedCultures = new[]
//{
// new CultureInfo("en-US"),
// new CultureInfo("fr"),
//};
//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("en-US"),
//    // Formatting numbers, dates, etc.
//    SupportedCultures = supportedCultures,
//    // UI strings that we have localized.
//    SupportedUICultures = supportedCultures
//});

app.MapGet("/", () => "Hello World!");
    

app.MapGet("/medications", async (SmartMedMedicationDbContext dbContext) =>
{
    var medications = await dbContext.Medications.ToListAsync();
    return medications;
}).WithTags("Home")
    .WithMetadata(new SwaggerOperationAttribute("Home", "Base Get Endpoint"))
    .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status200OK, "Success!"))
    .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized, "You're not Authorized!"))
    .WithMetadata(new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError, "Failed!"))
    .RequireAuthorization(); ;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Edge Configuration API v1"));
}

app.UseHttpsRedirection();
app.UseCors();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.Headers[AuthSchemeConstants.SmartMedAuthFailureHeader].Count != 0)
    {
        var detail = context.Response.Headers[AuthSchemeConstants.SmartMedAuthFailureHeader].FirstOrDefault();
        await SmartMedAuthExtensions.ValidationProblemResponse(
            context,
            detail ?? AuthSchemeConstants.DefaultAuthFailureMessage,
            new Dictionary<string, string[]>
            {
                [AuthSchemeConstants.SmartMedAuthFailureHeader] = [detail!.ToString()]
            });
    }
});

app.UseAuthorization();
app.UseStatusCodePages();

app.Run();
