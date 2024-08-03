using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Settings;

namespace VideoToPostGenerationAPI.Services;

/// <summary>
/// Service for handling token generation and verification.
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly TokenValidationParameters _tokenValidationParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="options">Options for JWT configuration.</param>
    /// <param name="unitOfWork">Unit of work for database operations.</param>
    /// <param name="userManager">User manager for user-related operations.</param>
    /// <param name="tokenValidationParameters">Parameters for validating JWT tokens.</param>
    public TokenService(IOptionsMonitor<JwtOptions> options, IUnitOfWork unitOfWork,
        UserManager<User> userManager, TokenValidationParameters tokenValidationParameters)
    {
        _options = options.CurrentValue;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _tokenValidationParameters = tokenValidationParameters;
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A tuple containing the generated JWT token and its expiration date.</returns>
    public async Task<(string JwtToken, DateTime ExpireDate)> GenerateJwtTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = Encoding.ASCII.GetBytes(_options.SigningKey);

        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!), // unique id
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by refresh token
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.ExpireTime),
            Audience = _options.ValidAudience,
            Issuer = _options.ValidIssuer,
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return await Task.FromResult((JwtToken: jwtToken, ExpireDate: token.ValidTo));
    }

    /// <summary>
    /// Verifies the specified JWT token.
    /// </summary>
    /// <param name="jwtToken">The JWT token to verify.</param>
    /// <returns>A tuple containing a message and a boolean indicating whether the token is valid.</returns>
    public async Task<(string Message, bool IsSuccess)> VerfiyTokenAsync(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var claimPrincipal = tokenHandler.ValidateToken(jwtToken, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                return (Message: "Token validation failed.", IsSuccess: false);

            var checkAlgorithm = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature);
            if (!checkAlgorithm)
                return (Message: "Token validation failed.", IsSuccess: false);

            var didParsed = long.TryParse
            (
                claimPrincipal.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp)?.Value,
                out long utcExpiryDate
            );
            if (!didParsed)
                return (Message: "Token validation failed.", IsSuccess: false);

            var jtiId = claimPrincipal.Claims.SingleOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jtiId is null)
                return (Message: "Token validation failed.", IsSuccess: false);

            return await Task.FromResult((Message: "Token is valid.", IsSuccess: true));
        }
        catch (Exception)
        {
            // Log the exception (if you have a logger)
            // _logger.LogError(ex, "Error occurred while verifying token.");

            return (Message: "Token validation failed.", IsSuccess: false);
        }
    }
}
