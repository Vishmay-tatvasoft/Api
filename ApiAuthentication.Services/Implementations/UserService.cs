using ApiAuthentication.Entity.Models;
using ApiAuthentication.Entity.ViewModels;
using ApiAuthentication.Repositories.Interfaces;
using ApiAuthentication.Services.Interfaces;

namespace ApiAuthentication.Services.Implementations;

public class UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IGenericRepository<User> userGR) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IGenericRepository<User> _userGR = userGR;


    #region Validate User Credentials
    public async Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM)
    {
        User userExists = await _userRepository.ValidateCredentials(loginVM.Email, loginVM.Password);
        if (userExists != null)
        {
            string accessToken = _jwtTokenService.GenerateJwtToken(loginVM.Email, userExists.Id.ToString(), loginVM.RememberMe);
            string refreshToken = _jwtTokenService.GenerateRefreshTokenJwt(loginVM.Email, userExists.Id.ToString(), loginVM.RememberMe);
            return new ApiResponseVM<object>(200, "Login successful", new TokenResponseVM{ Email = loginVM.Email, RememberMe = loginVM.RememberMe, AccessToken = accessToken, RefreshToken = refreshToken });
        }
        else
        {
            return new ApiResponseVM<object>(401, "Invalid email or password", null);
        }
    }
    #endregion

    #region Refresh Token
    public async Task<ApiResponseVM<object>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM)
    {
        if(string.IsNullOrEmpty(refreshTokenVM.RefreshToken))
        {
            return new ApiResponseVM<object>(400, "Refresh token is required", null);
        }
        if(_jwtTokenService.IsRefreshTokenValid(refreshTokenVM.RefreshToken))
        {
            string userID = _jwtTokenService.GetClaimValue(refreshTokenVM.RefreshToken, "UserID");
            User user = await _userGR.GetRecordById(Guid.Parse(userID));
            string newAccessToken = _jwtTokenService.GenerateJwtToken(user.Email, user.Id.ToString(), refreshTokenVM.RememberMe);
            string newRefreshToken = _jwtTokenService.GenerateRefreshTokenJwt(user.Email, user.Id.ToString(), refreshTokenVM.RememberMe);
            return new ApiResponseVM<object>(200, "Token refreshed successfully", new TokenResponseVM{ Email = user.Email, RememberMe = refreshTokenVM.RememberMe, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
        return new ApiResponseVM<object>(401, "Invalid refresh token", null);
    }
    #endregion
}
