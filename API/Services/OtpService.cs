using API.DTOs.Auth;
using API.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using Twilio;
using Twilio.TwiML.Messaging;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.Extensions.Caching.Memory;


namespace API.Services
{
    public class OtpService : IOTPService
    {

        private readonly SmsSettings _smsSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public OtpService(IOptions<SmsSettings> smsOptions, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _smsSettings = smsOptions.Value;
            _memoryCache = memoryCache;
            _httpClient = httpClient;
        }

        public async Task<OtpResponse> SendOtpAsync(string phoneNumber)
        {
            var otp = GenerateOtp();
            var url = $"{_smsSettings.ApiUrl}?loginName={_smsSettings.LoginName}" +
                      $"&sign={_smsSettings.Sign}" +
                      $"&serviceTypeId={_smsSettings.ServiceTypeId}" +
                      $"&phoneNumber={phoneNumber}" +
                      $"&message={otp}" +
                      $"&brandName={_smsSettings.BrandName}";

            var response = await _httpClient.GetAsync(url);


            var jsonString = await response.Content.ReadAsStringAsync();
            var smsResponse = JsonSerializer.Deserialize<OtpResponse>(jsonString);

            if (smsResponse?.Code == 106) // Success
            {
                _memoryCache.Set($"OTP_{phoneNumber}", otp, TimeSpan.FromMinutes(5));
            }

            return smsResponse;
        }

        public bool VerifyOtp(string phoneNumber, string otp)
        {
            if (_memoryCache.TryGetValue($"OTP_{phoneNumber}", out string cachedOtp))
            {
                return cachedOtp == otp;
            }

            throw new Exception("OTP đã hết hạn");
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
