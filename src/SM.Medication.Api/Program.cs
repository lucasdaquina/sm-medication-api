using Microsoft.EntityFrameworkCore;
using SM.Medication.Api.EndPoints;
using SM.Medication.Api.Extensions;
using SM.Medication.Auth;
using SM.Medication.Auth.Extensions;
using SM.Medication.Auth.Options;
using SM.Medication.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));

builder.SetupOpenApi();

builder.Services.AddDbContext<SmartMedMedicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MedicationDb")));

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = AuthSchemeConstants.SmartMedAuthScheme)
    .AddScheme<SMAuthSchemeOptions, SmartMedAuthenticationHandler>(AuthSchemeConstants.SmartMedAuthScheme, options => { });

builder.Services.AddAuthorization();

builder.AddInterfaces();
builder.SetupMapster();

var app = builder.Build();
app.MapMedicationEndPoints();

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
