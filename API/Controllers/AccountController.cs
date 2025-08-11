
using API.DTOs.Auth;
using API.IService;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AccountController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {

            if (request == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            var result = await _authService.Resgister(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Tên đăng nhập hoặc mật khẩu không được để trống." });
            }

            var result = await _authService.UserLogin(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { result.Message });
            }

            var refreshToken = result.Token.RefreshToken;

            Console.WriteLine(refreshToken);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            };

            Response.Cookies.Append("User_refreshToken", refreshToken, cookieOptions);


            return Ok(new { UserInfo = result.User, AccessToken = result.Token.Accesstoken });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["User_refreshToken"];
                Console.WriteLine(refreshToken);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("Refresh token không tồn tại trong request.");
                    return Unauthorized(new { Message = "Token không tồn tại." });
                }

                var result = await _authService.RefreshToken(refreshToken);
                if (!result.IsSuccess)
                {
                    return Unauthorized(new { result.Message });
                }

                Response.Cookies.Append("User_refreshToken", result.Token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Path = "/"
                });

                return Ok(new { AccessToken = result.Token.Accesstoken });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong RefreshToken: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Đã xảy ra lỗi hệ thống." });
            }
        }

    }
}
