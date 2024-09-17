using System.Security.Claims;
using UmojaCampus.Business.Entities;
using UmojaCampus.Shared.DTO.Account;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Contracts
{
    public interface IAuthService
    {
        Task<IResult<AccessToken>> LoginAsync(LoginDto dto);
        Task<string> GenerateAccessTokenAsync();
        string GenerateRefreshTokenAsync();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<IResult<AccessToken>> RefreshTokenAsync(RefreshTokenDto dto);
        Task<IResult> RevokeTokenAsync(CurrentUser user);
    }
}

