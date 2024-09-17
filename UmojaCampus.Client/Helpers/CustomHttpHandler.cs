using System.Net;
using UmojaCampus.Client.Services.Contracts;
using UmojaCampus.Shared.DTO.Account;

namespace UmojaCampus.Client.Helpers
{
    public class CustomHttpHandler(
        LocalStorageService localStorageService,
        IUserAccountService userAccountService): DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var _publicUrls = new[] { "login", "forgot-password", "reset-password", "refresh-token" };
            bool isPublicUrl = _publicUrls.Any(path => request.RequestUri.AbsoluteUri.Contains(path));
            bool loginUrl = request.RequestUri.AbsoluteUri.Contains("login");
            bool refreshTokenRul = request.RequestUri.AbsoluteUri.Contains("refresh-token");

            if (loginUrl || refreshTokenRul)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var result = await base.SendAsync(request, cancellationToken);
            if(result.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Get token from Local Storage
                var stringToken = await localStorageService.GetTokenAsync();
                if (stringToken is null) return result;

                //Check if the header contains token
                string token = string.Empty;
                try
                {
                    token = request.Headers.Authorization.Parameter;
                }
                catch {}

                var deserializeToken = Serializations.DeserializeJsonString<AccessToken>(stringToken);
                
                if (deserializeToken is null) return result;
                
                if(string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", deserializeToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                //Call refresh token endpoint
                var newJwtToken = await GetRefreshToken(new RefreshTokenDto
                {
                    Token = deserializeToken.Token,
                    RefreshToken = deserializeToken.RefreshToken
                });

                if (newJwtToken is null) return result;

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return result;
        }

        private async Task<string> GetRefreshToken(RefreshTokenDto token)
        {
            var result = await userAccountService.RefreshTokenAsync(token);

            var serializedToken = Serializations
                .SerializeObject(new AccessToken
                {
                    Token = result.Data.Token,
                    RefreshToken = result.Data.RefreshToken
                });

            await localStorageService.SetTokenAsync(serializedToken);
            
            return result.Data.Token;
        }
    }
}
