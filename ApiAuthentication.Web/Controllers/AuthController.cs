using ApiAuthentication.Entity.ViewModels;
using ApiAuthentication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiAuthentication.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignupVM signupVM)
    {
        if (signupVM == null)
        {
            return BadRequest("Invalid signup request");
        }
        ApiResponseVM<object> response = await _userService.RegisterUserAsync(signupVM);
        if (response.StatusCode == 201)
        {
            return Ok(response);
        }
        else if (response.StatusCode == 409)
        {
            return Conflict(response); // User already exists
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
    {
        if (loginVM == null)
        {
            return BadRequest("Invalid login request");
        }
        ApiResponseVM<object> response = await _userService.ValidateCredentialsAsync(loginVM);
        if (response.StatusCode == 200)
        {
            TokenResponseVM tokenResponse = (TokenResponseVM)response.Data!;
            DateTime expirationTime = tokenResponse.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7);
            SetCookie("DemoAccessToken", tokenResponse.AccessToken, expirationTime);
            SetCookie("DemoRefreshToken", tokenResponse.RefreshToken, expirationTime);
            return Ok(response);
        }
        else
        {
            return Unauthorized(response);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh()
    {
        RefreshTokenVM refreshTokenVM = new()
        {
            RefreshToken = Request.Cookies["DemoRefreshToken"],
            RememberMe = Convert.ToBoolean(_jwtTokenService.GetClaimValue(Request.Cookies["DemoRefreshToken"]!, "RememberMe")),
        };
        ApiResponseVM<object> response = await _userService.RefreshTokenAsync(refreshTokenVM);
        if (response.StatusCode == 200)
        {
            TokenResponseVM tokenResponse = (TokenResponseVM)response.Data;
            DateTime expirationTime = tokenResponse.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7);
            SetCookie("DemoAccessToken", tokenResponse.AccessToken, expirationTime);
            SetCookie("DemoRefreshToken", tokenResponse.RefreshToken, expirationTime);
            return Ok(response);
        }
        else if (response.StatusCode == 400)
        {
            return BadRequest(response);
        }
        else
        {
            return Unauthorized(response);
        }
    }

    #region Logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        RemoveCookie("DemoAccessToken");
        RemoveCookie("DemoRefreshToken");
        return Ok(new ApiResponseVM<object>(200, "Logged out successfully", null));
    }
    #endregion

    private void SetCookie(string name, string value, DateTime expiryTime)
    {
        Response.Cookies.Append(name, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expiryTime
        });
    }

    private void RemoveCookie(string name)
    {
        Response.Cookies.Delete(name, new CookieOptions
        {
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
}
