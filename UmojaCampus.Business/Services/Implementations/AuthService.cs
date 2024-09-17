using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UmojaCampus.Business.Entities;
using UmojaCampus.Business.Services.Contracts;
using UmojaCampus.Shared.Configuration;
using UmojaCampus.Shared.DTO.Account;
using UmojaCampus.Shared.Resources;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Business.Services.Implementations
{
    public class AuthService(IOptions<JwtConfiguration> options,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager) : IAuthService
    {
        private readonly JwtConfiguration _jwt = options.Value;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private ApplicationUser _user;
        public async Task<string> GenerateAccessTokenAsync()
        {
            var signingCredentials = GetSignInCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<IResult<AccessToken>> LoginAsync(LoginDto dto)
        {
            _user = await _userManager.FindByEmailAsync(dto.Email);

            if (_user == null)
            {
                return await Result<AccessToken>.FailAsync(ErrorResource.UserNotFound);
            }

            if (!_user.EmailConfirmed || _user.IsDeleted)
            {
                return await Result<AccessToken>.FailAsync(ErrorResource.UserNotFound);
            }

            var result = await _signInManager.PasswordSignInAsync(
                _user,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return await Result<AccessToken>.FailAsync(ErrorResource.AccountLockedOut);
            }

            if (!result.Succeeded)
            {
                return await Result<AccessToken>.FailAsync(ErrorResource.InvalidEmailOrPassword);
            }

            var refreshToken = GenerateRefreshTokenAsync();

            _user.LastLoginDate = DateTime.UtcNow;
            _user.RefreshToken = refreshToken;
            _user.RefreshTokenExpiryDate = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(_user);

            var token = await GenerateAccessTokenAsync();

            var accessToken = new AccessToken
            {
                Token = token,
                RefreshToken = refreshToken
            };

            return await Result<AccessToken>.SuccessAsync(accessToken);
        }
       
        public string GenerateRefreshTokenAsync()
        {
            var randomNumber = new Byte[32];
            using (var range = RandomNumberGenerator.Create())
            {
                range.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey)),
                ValidateLifetime = false, // We don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken =  securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<IResult<AccessToken>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var accessToken = dto.Token;
            var refreshToken = dto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);

            var userId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            
            _user ??= await _userManager.FindByIdAsync(userId);

            if (_user is null ||
                _user.RefreshToken != refreshToken ||
                _user.RefreshTokenExpiryDate <= DateTime.Now)
            {
                return await Result<AccessToken>.FailAsync("Invalid request.");
            }

            var newAccessToken = await GenerateAccessTokenAsync();
            var newRefreshToken = GenerateRefreshTokenAsync();

            _user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(_user);

            return await Result<AccessToken>.SuccessAsync(new AccessToken
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<IResult> RevokeTokenAsync(CurrentUser user)
        {
            _user ??= await _userManager.FindByIdAsync(user.Id);

            if (_user is null)
            {
                return await Result<string>.FailAsync();
            }

            _user.RefreshToken = null;
            await _userManager.UpdateAsync(_user);

            return await Result<string>.SuccessAsync();
        }

        #region Private Methods
        private SigningCredentials GetSignInCredentials()
        {
            var secretKey = _jwt.SecretKey;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, _user.Id),
                new(JwtRegisteredClaimNames.Email, _user.Email),
                new(JwtRegisteredClaimNames.Name, _user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(_user);
            roles.ToList().ForEach(role => claims.Add(new Claim("roles", role)));

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var issuer = _jwt.ValidIssuer;
            var audience = _jwt.ValidAudience;
            var expires = _jwt.ExpiresIn;
            var tokenOptions = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(expires)),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        #endregion
    }
}
