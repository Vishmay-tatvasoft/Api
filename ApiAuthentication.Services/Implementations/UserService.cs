using ApiAuthentication.Entity.Models;
using ApiAuthentication.Entity.Shared;
using ApiAuthentication.Entity.ViewModels;
using ApiAuthentication.Repositories.Interfaces;
using ApiAuthentication.Services.Interfaces;
using Isopoh.Cryptography.Argon2;

namespace ApiAuthentication.Services.Implementations;

public class UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IGenericRepository<User> userGR) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IGenericRepository<User> _userGR = userGR;


    #region Validate User Credentials
    public async Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM)
    {
        FhUser? user = await _userRepository.GetUserByEmail(loginVM.Email);
        if (user == null || Argon2.Verify(loginVM.Password, user.Password))// encrypted password verification here
        {
            return new ApiResponseVM<object>(401, Constants.INVALID_CREDENTIALS, null);
        }
        else
        {
            string accessToken = _jwtTokenService.GenerateJwtToken(loginVM.Email, user.UserId.ToString(), loginVM.RememberMe);
            string refreshToken = _jwtTokenService.GenerateRefreshTokenJwt(loginVM.Email, user.UserId.ToString(), loginVM.RememberMe);
            return new ApiResponseVM<object>(200, string.Concat(Constants.LOGIN, Constants.SUCCESSFULLY), new TokenResponseVM { Email = loginVM.Email, RememberMe = loginVM.RememberMe, AccessToken = accessToken, RefreshToken = refreshToken });
        }
    }
    #endregion

    #region Refresh Token
    public async Task<ApiResponseVM<object>> RefreshTokenAsync(RefreshTokenVM refreshTokenVM)
    {
        if (string.IsNullOrEmpty(refreshTokenVM.RefreshToken))
        {
            return new ApiResponseVM<object>(400, Constants.REFRESH_TOKEN_REQUIRED, null);
        }
        if (_jwtTokenService.IsRefreshTokenValid(refreshTokenVM.RefreshToken))
        {
            string userID = _jwtTokenService.GetClaimValue(refreshTokenVM.RefreshToken, "UserID");
            User? user = await _userGR.GetRecordById(Guid.Parse(userID)) ?? null;
            if (user != null)
            {
                string newAccessToken = _jwtTokenService.GenerateJwtToken(user.Email, user.Id.ToString(), refreshTokenVM.RememberMe);
                string newRefreshToken = _jwtTokenService.GenerateRefreshTokenJwt(user.Email, user.Id.ToString(), refreshTokenVM.RememberMe);
                return new ApiResponseVM<object>(200, Constants.TOKEN_REFRESHED, new TokenResponseVM { Email = user.Email, RememberMe = refreshTokenVM.RememberMe, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            return new ApiResponseVM<object>(401, Constants.USER_NOT_EXIST, null);
        }
        return new ApiResponseVM<object>(401, Constants.INVALID_REFRESH_TOKEN, null);
    }
    #endregion
}
