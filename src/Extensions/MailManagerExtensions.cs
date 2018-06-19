using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Company.WebApplication1.Services.Mail;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MailManagerExtensions
    {
        public static Task SendEmailConfirmationAsync(this IMailManager emailManager, string email, string link)
        {
            return emailManager.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.");
        }

        public static Task SendResetPasswordAsync(this IMailManager emailManager, string email, string callbackUrl)
        {
            return emailManager.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
