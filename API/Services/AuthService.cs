using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.IService;
using API.DTOs.Response;
using API.DTOs.Auth;
using API.Models;
using FluentValidation;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Twilio.Rest.Api.V2010.Account;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly IValidator<RegisterDTO> _validator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;


        public AuthService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, ILogger<AuthService> logger, IValidator<RegisterDTO> v, IUserRepository userRepository, RoleManager<IdentityRole> roleManager    )
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _validator = v;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        public async Task<bool> ChangePassword(ResetPassDTO resetPassDTO)
        {
            if (resetPassDTO.NewPassword != resetPassDTO.ConfirmPassword)
                throw new Exception("Mật khẩu xác nhận không khớp");

            var user = await _userManager.FindByIdAsync(resetPassDTO.UserID);
            CheckUserNull(user);

            var checkPasswordResult =  await _userManager.CheckPasswordAsync(user, resetPassDTO.OldPassword);
            if (!checkPasswordResult)
                throw new Exception("Mật khẩu cũ không chính xác");

            var changePassResult = await _userManager.ChangePasswordAsync(user, resetPassDTO.OldPassword, resetPassDTO.NewPassword);
            if (!changePassResult.Succeeded)
            {
                _logger.LogWarning($"Đổi mật khẩu thất bại userId {user.Id} : " + string.Join(" | ", changePassResult.Errors.Select(e => e.Description)));
                throw new Exception("Đổi mật khẩu thất bại , vui lòng thử lại sau");
            }


            if (user.FisrtLogin)
            {
                user.FisrtLogin = false;
                await _userManager.UpdateAsync(user);
            }

            return true ; 

        }


        private void CheckUserNull(User user)
        {
            if (user == null)
                throw new Exception("User không tồn tại"); 
        }

        public async Task<AuthResponse> AdminLogin(LoginDTO request)
        {
            try
            {
                _logger.LogInformation("Login attempt for username: {UserName}", request.Email);

                var user = await _userManager.FindByEmailAsync(request.Email);
                CheckUserNull(user);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("User") || roles == null)
                    return Failure("Tài khoản không có quyền truy cập");


                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!loginResult.Succeeded)
                {
                    _logger.LogWarning($"Đăng nhập thất bại Id tài khoản : {user.Id}");
                    return Failure("Sai mật khẩu");
                }


                //Nếu đăng nhập lần đầu , trả về token để đổi pass
                if (user.FisrtLogin)
                {
                    var changePassToken = _tokenService.GenerateChangePasswordToken(user);
                    return new AuthResponse
                    {
                        IsSuccess = true,
                        FirstLogin = user.FisrtLogin,
                        Message = "Đăng nhập lần đầu, vui lòng đổi mật khẩu để tiếp tục",
                        Token = changePassToken

                    };
                }

                _logger.LogInformation($"Login successfully user :  {user.Id}");

                
                var token = await _tokenService.GenerateToken(user, roles);
                return Success("Đăng nhập thành công", token,Role : roles.FirstOrDefault()  );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi đăng nhập tài khoản :  {UserName}", request.Email + ex.InnerException?.Message  );
                return Failure("Xảy ra lỗi :" + ex.Message);
            }
        }
        public async Task<AuthResponse> UserLogin(UserLoginDTO request)
        {
            try
            {
                _logger.LogInformation("Login attempt for username: {UserName}", request.PhoneNumber);

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                CheckUserNull(user);

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("User"))
                    return Failure("Tài khoản không có quyền truy cập");

                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!loginResult.Succeeded)
                {
                    _logger.LogWarning($"Đăng nhập thất bại Id tài khoản : {user.Id}");
                    return Failure("Sai mật khẩu");
                }

                _logger.LogInformation($"Login successfully user :  {user.Id}");
                var token = await _tokenService.GenerateToken(user,await GetRoles(user));
                return Success("Đăng nhập thành công", token,null ,  await _userRepository.GetUserByIdAsync(user.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi đăng nhập tài khoản :  {UserName}", request.PhoneNumber + ex.InnerException?.Message);
                return Failure("Xảy ra lỗi :" + ex.Message);
            }
        }


        private AuthResponse Success(string Message, TokenDTO token,string Role , UserInfo? user = null)
        {
        
            return new AuthResponse
            {
                IsSuccess = true,
                Token = token,
                Message = Message,
                User = user,
                Role = Role 
            };
        }

        private AuthResponse Failure(string Message)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Token = null,
                Message = Message

            };
        }


        public async Task Logout(string refreshToken)
        {
            await _tokenService.RevokeToken(refreshToken);

            await _signInManager.SignOutAsync();
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken)
        {
            try
            {
                var token = await _tokenService.CheckFreshToken(refreshToken);
                // Kiểm tra refresh token từ cơ sở dữ liệu hoặc bộ nhớ
                if (token is null)
                {
                    return Failure("Refresh token không hợp lệ hoặc đã hết hạn");
                }

                // Tìm người dùng theo UserID
                var user = await _userManager.FindByIdAsync(token.UserId);
                CheckUserNull(user); 

                // Tạo lại access token và refresh token mới
                var newToken = await _tokenService.GenerateToken(user, await GetRoles(user));


                return Success("Đăng nhập thành công", newToken,null,null);
            }
            catch (Exception ex)
            {
                return Failure("Xảy ra lỗi :" + ex.Message);
            }
        }

        private async Task<IList<string>> GetRoles(User user) => await _userManager.GetRolesAsync(user);

        public async Task add(string email , string pass) 
        {
            var user = new User()
            {
                Email = email,
                UserName = "AdminTest",
                Name = "Testing"
               
            };
            await _userManager.CreateAsync(user, pass);

            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole("Admin");
                await _roleManager.CreateAsync(adminRole);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

        }

        public async Task<AuthResponse> Resgister(RegisterDTO request)
        {
            try
            {
                var valid = await _validator.ValidateAsync(request);
                if (!valid.IsValid)
                    return Failure(string.Join(", ", valid.Errors));


                var newUser = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.PhoneNumber,
                    Name = request.Name,
                    FisrtLogin = false
                };


                var result = await _userManager.CreateAsync(newUser, request.Password).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    return Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Kiểm tra và tạo vai trò "User" nếu chưa tồn tại
                var userRole = await _roleManager.FindByNameAsync("User");
                if (userRole == null)
                {
                    userRole = new IdentityRole("User");
                    await _roleManager.CreateAsync(userRole);
                }


                await _userManager.AddToRoleAsync(newUser, "User").ConfigureAwait(false);
                _logger.LogInformation($"Đăng kí thành công {newUser.Id}");

                return Success("Đăng kí thành công", null,null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi đăng kí tài khoản :  {UserName}", request.PhoneNumber + ex.InnerException?.Message);
                return Failure("Xảy ra lỗi :" + ex.Message);
            }
        }
    }
}
