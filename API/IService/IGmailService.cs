﻿namespace API.IService
{
    public interface IGmailService
    {
        public Task SendEmailAsync(string to, string subject, string body);


    }
}
