﻿using API.DTOs.Auth;

namespace API.DTOs.Response
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public TokenDTO Token { get; set; }
        public UserInfo? User { get; set; } 
        public string Role { get; set; } 
        public bool FirstLogin { get; set; }
        public string? Message { get; set; }
    }
}
