using Microsoft.AspNetCore.Identity.UI.Services;

namespace IMS.Infrastructure.Services
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string Email, string subject, string htmlMessage) 
        { 
        return Task.CompletedTask;
        }
    }

}
