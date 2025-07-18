using ApiAuthentication.Entity.Models;
using ApiAuthentication.Entity.Shared;
using ApiAuthentication.Entity.ViewModels;
using ApiAuthentication.Repositories.Interfaces;
using ApiAuthentication.Services.Interfaces;
using Isopoh.Cryptography.Argon2;

namespace ApiAuthentication.Services.Implementations;

public class UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IGenericRepository<FhUser> userGR) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IGenericRepository<FhUser> _userGR = userGR;

    #region Register User
    public async Task<ApiResponseVM<object>> RegisterUserAsync(SignupVM signupVM)
    {
        if (string.IsNullOrEmpty(signupVM.EmailAddress) || string.IsNullOrEmpty(signupVM.Password))
        {
            return new ApiResponseVM<object>(400, Constants.INVALID_SIGNUP_REQUEST, null);
        }

        FhUser? existingUser = await _userRepository.GetUserByEmail(signupVM.EmailAddress);
        if (existingUser != null)
        {
            return new ApiResponseVM<object>(409, Constants.USER_ALREADY_EXISTS, null);
        }

        // Encrypt the password before saving
        string encryptedPassword = Argon2.Hash(signupVM.Password);

        FhUser newUser = new()
        {
            EmailAddress = signupVM.EmailAddress,
            Password = encryptedPassword,
            FirstName = signupVM.FirstName,
            LastName = signupVM.LastName,
            UserName = signupVM.UserName,
            PhoneNumber = signupVM.PhoneNumber,
            RoleId = signupVM.RoleId,
            UserType = signupVM.UserType,
            UserId = ""
        };

        await _userGR.AddRecord(newUser);
        await _userGR.SaveChangesAsync();
        return new ApiResponseVM<object>(201, Constants.USER_REGISTERED_SUCCESSFULLY, newUser);
    }
    #endregion

    #region Validate User Credentials
    public async Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM)
    {
        FhUser? user = await _userRepository.GetUserByEmail(loginVM.Email);
        bool res = Argon2.Verify(user.Password, loginVM.Password);
        if (user == null || !Argon2.Verify(user.Password, loginVM.Password)) // encrypted password verification here
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
            FhUser? user = await _userGR.GetRecordById(userID);
            if (user != null)
            {
                string newAccessToken = _jwtTokenService.GenerateJwtToken(user.EmailAddress, user.UserId.ToString(), refreshTokenVM.RememberMe);
                string newRefreshToken = _jwtTokenService.GenerateRefreshTokenJwt(user.EmailAddress, user.UserId.ToString(), refreshTokenVM.RememberMe);
                return new ApiResponseVM<object>(200, Constants.TOKEN_REFRESHED, new TokenResponseVM { Email = user.EmailAddress, RememberMe = refreshTokenVM.RememberMe, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            return new ApiResponseVM<object>(401, Constants.USER_NOT_EXIST, null);
        }
        return new ApiResponseVM<object>(401, Constants.INVALID_REFRESH_TOKEN, null);
    }
    #endregion
}
