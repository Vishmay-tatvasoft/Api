using ApiAuthentication.Entity.ViewModels;

namespace ApiAuthentication.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponseVM<object>> RegisterUserAsync(SignupVM signupVM);
    Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM);
    Task<ApiResponseVM<object>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM);
<<<<<<< HEAD
    Task<ApiResponseVM<object>> ForgotPasswordAsync(string email, string username);
    Task<ApiResponseVM<object>> ResetPasswordAsync(ResetPassVM resetPassVM);
=======
    Task<ApiResponseVM<object>> GetUserByUserNameAsync(string userName);
>>>>>>> 65e10f7d343ad97dc1d1e7d2c7c00d04e8343e10
}
