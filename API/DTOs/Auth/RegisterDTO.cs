namespace API.DTOs.Auth
{
    public class RegisterDTO
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set;  }

        public string Password { get; set;  }

        public string ConfirmPassword { get; set; }
    }
}
