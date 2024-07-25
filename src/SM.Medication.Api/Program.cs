using SM.Medication.Api.Extensions;
using SM.Medication.Auth;
using SM.Medication.Auth.Extensions;
using SM.Medication.Auth.Options;
using SmartMed.Medication.Auth.Constants;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));

builder.Services.SetupOpenApi();

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = AuthSchemeConstants.SmartMedAuthScheme)
    .AddScheme<SMAuthSchemeOptions, SmartMedAuthenticationHandler>(AuthSchemeConstants.SmartMedAuthScheme, options => { });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/",
    [SwaggerOperation(
        Summary = "Home",
        Description = "Base Get Endpoint")]
    [SwaggerResponse(200, "Success!")]
    [SwaggerResponse(401, "You're not Authorized!")]
    [SwaggerResponse(500, "Failed!")]
    () 
        => "Hello World!")
    .WithTags("Home")
    .RequireAuthorization();

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
