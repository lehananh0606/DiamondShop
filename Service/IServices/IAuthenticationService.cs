

using Service.ViewModels.AccountToken;
using Service.ViewModels.AcountToken;
using Service.ViewModels.Request;
using Service.ViewModels.Response;

namespace Service.IServices
{
    public interface IAuthenticationService
    {
        public Task<AccountResponse> LoginAsync(AccountRequest accountRequest, JWTAuth jwtAuth);
        public Task<AccountTokenResponse> ReGenerateTokensAsync(AccountTokenRequest accountTokenRequest, JWTAuth jwtAuth);
       
    }
}
