using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IServices;

public interface ITokenService
{
    Task<(string JwtToken, DateTime ExpireDate)> GenerateJwtTokenAsync(User user);

    Task<(string Message, bool IsSuccess)> VerfiyTokenAsync(string jwtToken);
}
