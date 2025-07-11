﻿
using API.DTOs.Response;
using API.DTOs.Auth;

namespace API.IService
{
    public interface IAuthService
    {
        public Task<AuthResponse> AdminLogin(LoginDTO request);

        public Task<AuthResponse> UserLogin(UserLoginDTO request);

        public Task<AuthResponse> Resgister(RegisterDTO request);

        public Task<AuthResponse> RefreshToken(string refreshToken);

        public Task add(string email, string pass); 

        public Task<bool> ChangePassword(ResetPassDTO resetPassDTO);
        public Task Logout(string refreshToken);
    }
}
