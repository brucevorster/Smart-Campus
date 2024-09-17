using System.Net.Http.Json;
using UmojaCampus.Client.Helpers;
using UmojaCampus.Client.Services.Contracts;
using UmojaCampus.Shared.DTO.Account;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Client.Services.Implementations
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string BaseAuthUrl = "api/auth";
        public async Task<Result<AccessToken>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{BaseAuthUrl}/refresh-token", refreshTokenDto);
            
            if(!result.IsSuccessStatusCode)
            {
                return Result<AccessToken>.Fail(result.ReasonPhrase);
            }

            var resultContent = await result.Content.ReadFromJsonAsync<AccessToken>();

            return await Result<AccessToken>.SuccessAsync(resultContent);
        }

        public async Task<Result<AccessToken>> SignInAsync(LoginDto loginDto)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{BaseAuthUrl}/login", loginDto);

            if (!result.IsSuccessStatusCode)
                return Result<AccessToken>.Fail("There was an error!");

            var resultContent = await result.Content.ReadFromJsonAsync<AccessToken>();
            return await Result<AccessToken>.SuccessAsync(resultContent);
        }
    }
}
