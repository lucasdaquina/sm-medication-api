using Microsoft.AspNetCore.Authentication.OAuth;
using SM.Medication.Api.Extensions;
using SM.Medication.Auth;
using SM.Medication.Auth.Options;
using SmartMed.Medication.Api.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.Services.AddCors(options => options.AddPolicy("allowAny", o => o.AllowAnyOrigin()));

builder.Services.SetupOpenApi();

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = AuthSchemeConstants.SmartMedAuthScheme)
    .AddScheme<SMAuthSchemeOptions, SmartMedAuthenticationHandler>(AuthSchemeConstants.SmartMedAuthScheme, options => { });

builder.Services.AddAuthorization();

var app = builder.Build();


app.MapGet("/", () => "Hello World!").RequireAuthorization();

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
    if (context.Response.Headers[AuthSchemeConstants.SmartMedAuthFailureHeader].Any())
    {
        var detail = context.Response.Headers[AuthSchemeConstants.SmartMedAuthFailureHeader].FirstOrDefault();
        var validationProblem = Results.ValidationProblem(
          errors: new Dictionary<string, string[]> { [AuthSchemeConstants.SmartMedAuthFailureHeader] = new string[1] { detail!.ToString() } },
          detail: detail,
          instance: "SmartMedAuthHandler",
          statusCode: StatusCodes.Status401Unauthorized,
          title: "SmartMed Authentication Error",
          type: "https://datatracker.ietf.org/doc/html/rfc2616#section-10.4.2");

        await validationProblem.ExecuteAsync(context);
    }

});
app.Run();
