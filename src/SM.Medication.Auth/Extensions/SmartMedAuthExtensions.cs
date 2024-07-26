namespace SM.Medication.Auth.Extensions;

public static class SmartMedAuthExtensions
{
    private const string AUTH_HANDLER = nameof(SmartMedAuthExtensions);
    private const string VALIDATION_RESPONSE_TITLE = "SmartMed API Authentication Error";
    private const string RFC_401_LINK = "https://datatracker.ietf.org/doc/html/rfc2616#section-10.4.2";

    public static async Task ValidationProblemResponse(
        HttpContext context,
        string detail,
        Dictionary<string, string[]> errors)
    {
        if (context == null)
        {
            throw new ArgumentNullException(
                nameof(context),
                $"{nameof(SmartMedAuthExtensions)}: ValidationProblemResponse");
        }

        var validationProblem = Results.ValidationProblem(
          errors: errors,
          detail: detail,
          instance: AUTH_HANDLER,
          statusCode: StatusCodes.Status401Unauthorized,
          title: VALIDATION_RESPONSE_TITLE,
          type: RFC_401_LINK);

        await validationProblem.ExecuteAsync(context);

    }
}
