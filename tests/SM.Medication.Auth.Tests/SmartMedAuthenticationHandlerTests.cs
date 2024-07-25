using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using FakeItEasy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SM.Medication.Auth.Models;
using SM.Medication.Auth.Options;
using SmartMed.Medication.Auth.Constants;

namespace SM.Medication.Auth.Tests;

public class SmartMedAuthenticationHandlerTests
{
    private readonly IOptionsMonitor<SMAuthSchemeOptions> options;
    private readonly ILoggerFactory logger;
    private readonly UrlEncoder encoder;
    //private readonly Mock<IProvidePrincipal> _principalProvider;
    private readonly SmartMedAuthenticationHandler handler;

    public SmartMedAuthenticationHandlerTests()
    {
        options = A.Fake<IOptionsMonitor<SMAuthSchemeOptions>>();
        A.CallTo(() => options.Get(A<string>._)).Returns(new SMAuthSchemeOptions());

        logger = A.Fake<ILoggerFactory>();
        A.CallTo(() => logger.CreateLogger(A<string>._)).Returns(A.Fake<ILogger>());

        encoder = A.Fake<UrlEncoder>();

        handler = new SmartMedAuthenticationHandler(options, logger, encoder);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldSucceed_WhenTokenIsValid()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = "valid-token", Role = "user" };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"SmartMed {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.True(authResult.Succeeded);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldContainIdentity_WhenTokenIsValid()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = "valid-token", Role = "user" };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"SmartMed {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.NotNull(authResult.Ticket);
        Assert.NotNull(authResult.Ticket.Principal.Identity);
        Assert.True(authResult.Ticket.Principal.Identity.IsAuthenticated);
        Assert.Equal(AuthSchemeConstants.SmartMedAuthScheme, authResult.Ticket.Principal.Identity.AuthenticationType);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldContainClaims_WhenTokenIsValid()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = "valid-token", Role = "user" };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"SmartMed {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.NotNull(authResult.Ticket);
        Assert.NotNull(authResult.Ticket.Principal.Identity);
        var identity = authResult.Ticket.Principal.Identity as ClaimsIdentity;
        Assert.NotNull(identity);
        var claims = identity.Claims;
        Assert.Equal(2, claims.Count());
        Assert.Contains("valid-token", claims.Select(c => c.Value));
        Assert.Contains("user", claims.Select(c => c.Value));
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldFail_WhenTokenIsNotValid_Using_Bearer()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = "valid-token", Role = "user" };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"Bearer {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.False(authResult.Succeeded);
        Assert.NotNull(authResult.Failure);
        Assert.Equal("Invalid token provided", authResult.Failure.Message);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldFail_WhenTokenIsNotValid_Using_EmptyToken()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = string.Empty, Role = "user" };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"SmartMed {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.False(authResult.Succeeded);
        Assert.NotNull(authResult.Failure);
        Assert.Equal("The Token is not valid.", authResult.Failure.Message);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldFail_WhenTokenIsNotValid_Using_EmptyUser()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var tokenModel = new TokenModel { Token = "valid-token", Role = string.Empty };
        var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenModel)));
        context.Request.Headers.Authorization = $"SmartMed {tokenString}";

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.False(authResult.Succeeded);
        Assert.NotNull(authResult.Failure);
        Assert.Equal("The Token is not valid.", authResult.Failure.Message);
    }

    [Fact]
    public async Task HandleAuthenticateAsync_ShouldFail_WithoutHeader()
    {
        //Arrange
        var context = new DefaultHttpContext();

        await handler.InitializeAsync(
            new AuthenticationScheme(
                AuthSchemeConstants.SmartMedAuthScheme,
                displayName: null,
                typeof(Auth.SmartMedAuthenticationHandler)),
            context);
        _ = new AuthenticationScheme(AuthSchemeConstants.SmartMedAuthScheme, null, typeof(Auth.SmartMedAuthenticationHandler));

        //Act
        var authResult = await handler.AuthenticateAsync();

        //Assert
        Assert.False(authResult.Succeeded);
        Assert.NotNull(authResult.Failure);
        Assert.Equal("Auth header not found.", authResult.Failure.Message);
    }
}
