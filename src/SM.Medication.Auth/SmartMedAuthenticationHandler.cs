using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using SM.Medication.Auth.Models;
using SM.Medication.Auth.Options;
using SmartMed.Medication.Auth.Constants;

namespace SM.Medication.Auth;

/// <summary>
/// Handles authentication for the medication application.
/// </summary>
public class SmartMedAuthenticationHandler(
        IOptionsMonitor<SMAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<SMAuthSchemeOptions>(options, logger, encoder)
{
    private const string COMPUTERUID_SHA256_REGEX = "[A-Fa-f0-9]{64}";

    /// <summary>
    /// Handles the authentication process.
    /// </summary>
    /// <returns>The authentication result.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
        {
            return await ProcessAuthFailure("Auth header not found.");
        }

        var match = TokenValidator.Match(authorization.ToString());
        if (!match.Success)
        {
            return await ProcessAuthFailure("Invalid token provided");
        }

        var tokenValue = match.Groups["token"].Value;
        TokenModel tokenModel;
        try
        {
            var bytes = Convert.FromBase64String(tokenValue);
            var @string = Encoding.UTF8.GetString(bytes);
            tokenModel = JsonSerializer.Deserialize<TokenModel>(@string);
        }
        catch (Exception ex)
        {
            base.Logger.LogError("Exception occured while deserializing: {Exception}", ex);
            return await ProcessAuthFailure("TokenParseException");
        }

        //Validation should be done on the DB, using MedicalOptions to have access to connectionString, to return if token is valid or not
        //Or some checksum, or any type of validation
        //For now, we are just checking if the token is not empty
        if (string.IsNullOrWhiteSpace(tokenModel.Token))
        {
            return await ProcessAuthFailure("The Token is not valid.");
        }

        return await Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid", tokenModel.Token),
                                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role", tokenModel.Role),
                            ],
                            AuthSchemeConstants.SmartMedAuthScheme)
                        ),
                    base.Scheme.Name)));
    }

    private async Task<AuthenticateResult> ProcessAuthFailure(string failureMessage)
    {
        base.Response.StatusCode = StatusCodes.Status401Unauthorized;
        base.Response.Headers.Add(new KeyValuePair<string, StringValues>(AuthSchemeConstants.SmartMedAuthFailureHeader, failureMessage));
        return await Task.FromResult(AuthenticateResult.Fail(failureMessage));
    }
}

/// <summary>
/// Provides token validation functionality.
/// </summary>
public static partial class TokenValidator
{
    [GeneratedRegex(AuthSchemeConstants.SmartMedToken)]
    private static partial Regex MatchIfValidUrl();

    public static bool IsValidToken(string token)
        => MatchIfValidUrl().IsMatch(token);

    public static Match Match(string token)
        => MatchIfValidUrl().Match(token);
}

