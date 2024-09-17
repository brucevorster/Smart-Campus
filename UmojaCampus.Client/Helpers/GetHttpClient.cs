using UmojaCampus.Shared.DTO.Account;

namespace UmojaCampus.Client.Helpers
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory,
        LocalStorageService localStorageService)
    {
        private const string HeaderKey = "Authorization";
        private const string ClientName = "UmojaCampus";

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient(ClientName);
            var token = await localStorageService.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
                return client;

            var deserializeToken = Serializations.DeserializeJsonString<AccessToken>(token);
            if(deserializeToken is null)
                return client;

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);

            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient(ClientName);
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}
