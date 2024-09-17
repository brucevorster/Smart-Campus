using UmojaCampus.Shared.DTO.Account;
using UmojaCampus.Shared.Wrapper;

namespace UmojaCampus.Client.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<Result<AccessToken>> SignInAsync(LoginDto login);
        Task<Result<AccessToken>> RefreshTokenAsync(RefreshTokenDto token);
    }
}
