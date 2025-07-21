using ApiAuthentication.Entity.ViewModels;

namespace ApiAuthentication.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponseVM<object>> RegisterUserAsync(SignupVM signupVM);
    Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM);
    Task<ApiResponseVM<object>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM);
    Task<ApiResponseVM<object>> GetUserByUserNameAsync(string userName);
}
