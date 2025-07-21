using ApiAuthentication.Entity.Models;
using ApiAuthentication.Entity.Shared;
using ApiAuthentication.Entity.ViewModels;
using ApiAuthentication.Repositories.Interfaces;
using ApiAuthentication.Services.Interfaces;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Caching.Memory;

namespace ApiAuthentication.Services.Implementations;

public class UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IMailService mailService, IMemoryCache memoryCache, IGenericRepository<FhUser> userGR) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IGenericRepository<FhUser> _userGR = userGR;
    private readonly IMailService _mailService = mailService;
    private readonly IMemoryCache _memoryCache = memoryCache;

    #region Register User
    public async Task<ApiResponseVM<object>> RegisterUserAsync(SignupVM signupVM)
    {
        if (string.IsNullOrEmpty(signupVM.UserName) || string.IsNullOrEmpty(signupVM.FirstName) || string.IsNullOrEmpty(signupVM.LastName) || string.IsNullOrEmpty(signupVM.RoleId))
        {
            return new ApiResponseVM<object>(400, Constants.INVALID_SIGNUP_REQUEST, null);
        }

        FhUser? existingUser = await _userRepository.GetUserByUsername(signupVM.UserName);
        if (existingUser != null)
        {
            return new ApiResponseVM<object>(409, Constants.USER_ALREADY_EXISTS, null);
        }

        // OTP
        string otp = OneTimePasswordGenerator.GenerateAlphaNumericOtp();
        string hashedOtp = Argon2.Hash(otp);

        FhUser newUser = new()
        {
            Password = hashedOtp,
            FirstName = signupVM.FirstName,
            LastName = signupVM.LastName,
            UserName = signupVM.UserName,
            RoleId = signupVM.RoleId,
            UserType = signupVM.RoleId == "User" ? "U" : "R",
            UserId = ""
        };

        await _userGR.AddRecord(newUser);
        await _userGR.SaveChangesAsync();
        await _mailService.SendOtpEmail(signupVM.EmailAddress, signupVM.UserName, otp);
        return new ApiResponseVM<object>(201, Constants.USER_REGISTERED_SUCCESSFULLY, newUser);
    }
    #endregion

    #region Validate User Credentials
    public async Task<ApiResponseVM<object>> ValidateCredentialsAsync(LoginVM loginVM)
    {
        FhUser? user = await _userRepository.GetUserByUsername(loginVM.UserName);

<<<<<<< HEAD
        if (user == null)
        {
            return new ApiResponseVM<object>(404, Constants.USER_NOT_EXIST, null);
        }
        else if (!Argon2.Verify(user.Password, loginVM.Password)) // encrypted password verification here
=======
        if (user == null || !Argon2.Verify(user.Password, loginVM.Password)) // encrypted password verification here
>>>>>>> 65e10f7d343ad97dc1d1e7d2c7c00d04e8343e10
        {
            return new ApiResponseVM<object>(401, Constants.INVALID_CREDENTIALS, null);
        }
        else
        {
            string accessToken = _jwtTokenService.GenerateJwtToken(loginVM.UserName, user.UserId.ToString(), loginVM.RememberMe);
            string refreshToken = _jwtTokenService.GenerateRefreshTokenJwt(loginVM.UserName, user.UserId.ToString(), loginVM.RememberMe);
            return new ApiResponseVM<object>(200, string.Concat(Constants.LOGIN, Constants.SUCCESSFULLY), new TokenResponseVM { UserName = loginVM.UserName, RememberMe = loginVM.RememberMe, AccessToken = accessToken, RefreshToken = refreshToken });
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
                string newAccessToken = _jwtTokenService.GenerateJwtToken(user.UserName!, user.UserId.ToString(), refreshTokenVM.RememberMe);
                string newRefreshToken = _jwtTokenService.GenerateRefreshTokenJwt(user.UserName!, user.UserId.ToString(), refreshTokenVM.RememberMe);
                return new ApiResponseVM<object>(200, Constants.TOKEN_REFRESHED, new TokenResponseVM { UserName = user.UserName, RememberMe = refreshTokenVM.RememberMe, AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            return new ApiResponseVM<object>(401, Constants.USER_NOT_EXIST, null);
        }
        return new ApiResponseVM<object>(401, Constants.INVALID_REFRESH_TOKEN, null);
    }
    #endregion

<<<<<<< HEAD
    #region Forgot Password
    public async Task<ApiResponseVM<object>> ForgotPasswordAsync(string email, string username)
    {
        FhUser? user = await _userRepository.GetUserByUsername(username);
        if (user == null || string.IsNullOrEmpty(user.EmailAddress))
        {
            return new ApiResponseVM<object>(404, Constants.USER_NOT_EXIST, null);
        }
        else if (user.EmailAddress != email.ToLower())
        {
            return new ApiResponseVM<object>(400, Constants.INVALID_EMAIL, null);
        }
        else
        {
            string resetToken = _jwtTokenService.GenerateJwtToken(user.UserName, user.UserId, false);
            string cacheKey = $"reset_token:{resetToken}";
            _memoryCache.Set(cacheKey, user.EmailAddress.ToLower(), TimeSpan.FromMinutes(15)); // auto-expiry
            await _mailService.SendResetPasswordLink(user.EmailAddress, user.UserName, resetToken);
            return new ApiResponseVM<object>(200, Constants.RESET_LINK_SENT, null);
=======
    #region GetUserByUserNameAsync
    public async Task<ApiResponseVM<object>> GetUserByUserNameAsync(string userName)
    {
        FhUser? user = await _userRepository.GetUserByUsername(userName);
        if(user == null){
            return new ApiResponseVM<object>(404, Constants.USER_NOT_FOUND, null);
        }
        else
        {
            return new ApiResponseVM<object>(200, Constants.USER_FOUND, user);
>>>>>>> 65e10f7d343ad97dc1d1e7d2c7c00d04e8343e10
        }
    }
    #endregion

<<<<<<< HEAD
    #region Reset Password
    public async Task<ApiResponseVM<object>> ResetPasswordAsync(ResetPassVM resetPassVM)
    {
        string cacheKey = $"reset_token:{resetPassVM.Token}";

        if (_memoryCache.TryGetValue(cacheKey, out string? email))
        {
            if (_jwtTokenService.IsRefreshTokenValid(resetPassVM.Token))
            {
                string userID = _jwtTokenService.GetClaimValue(resetPassVM.Token, "UserID");
                FhUser? user = await userGR.GetRecordById(userID);
                if (user != null)
                {
                    user.Password = Argon2.Hash(resetPassVM.Password);
                    _userGR.UpdateRecord(user);
                    await _userGR.SaveChangesAsync();
                    _memoryCache.Remove(cacheKey); //allow for one time only
                    await _mailService.SendResetPasswordMessage(user.EmailAddress, user.UserName);
                    return new ApiResponseVM<object>(200, string.Join(" ", Constants.PASSWORD, Constants.RESET, Constants.SUCCESSFULLY), null);
                }
                else
                {
                    return new ApiResponseVM<object>(404, Constants.USER_NOT_EXIST, null);
                }
            }
        }
        return new ApiResponseVM<object>(410, Constants.INVALID_LINK, null);
    }
    #endregion
=======
>>>>>>> 65e10f7d343ad97dc1d1e7d2c7c00d04e8343e10
}
