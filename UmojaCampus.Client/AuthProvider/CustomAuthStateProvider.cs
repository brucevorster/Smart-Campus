using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UmojaCampus.Client.Helpers;
using UmojaCampus.Shared.DTO.Account;

namespace UmojaCampus.Client.AuthProvider
{
    public class CustomAuthStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorageService.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(new AuthenticationState(Anonymous));
            }

            var deserializeToken = Serializations
                .DeserializeJsonString<AccessToken>(token);

            if(deserializeToken == null)
            {
                return await Task.FromResult(new AuthenticationState(Anonymous));
            }

            var userClaims = DecryptToken(deserializeToken.Token);
            if(userClaims == null)
            {
                return await Task.FromResult(new AuthenticationState(Anonymous));
            }

            var claimsPrincipal = SetClaimPrincipal(userClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task UpdateAuthenticationState(AccessToken accessToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(accessToken.Token != null && accessToken.RefreshToken != null)
            {
                var serializeToken = Serializations.SerializeObject(accessToken);
                await localStorageService.SetTokenAsync(serializeToken);

                var getUserClaims = DecryptToken(accessToken.Token);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveTokenAsync();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims userClaims)
        {
            if(userClaims.Id == null ||
                userClaims.Email == null)
            {
                return new ClaimsPrincipal();
            }

            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                { 
                    new (ClaimTypes.NameIdentifier, userClaims.Id),
                    new (ClaimTypes.Email, userClaims.Email),
                    new (ClaimTypes.Role, userClaims.Role)
                },
                "JwtAuth"
            ));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if(string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            
            var userId = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var name = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            var role = token.Claims.FirstOrDefault(c => c.Type == "roles")?.Value;

            return new CustomUserClaims(userId, name, email, role);
        }
    }
}
