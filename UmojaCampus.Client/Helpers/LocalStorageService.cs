using Blazored.LocalStorage;

namespace UmojaCampus.Client.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string AuthToken = "auth-token";
        public async Task<string> GetTokenAsync()
            => await localStorageService.GetItemAsStringAsync(AuthToken);

        public async Task SetTokenAsync(string token)
            => await localStorageService.SetItemAsStringAsync(AuthToken, token);

        public async Task RemoveTokenAsync()
            => await localStorageService.RemoveItemAsync(AuthToken);
    }
}
