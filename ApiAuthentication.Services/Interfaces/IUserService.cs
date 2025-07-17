using ApiAuthentication.Entity.ViewModels;

namespace ApiAuthentication.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM);
    Task<ApiResponseVM<object>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM);
}
