using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WebApplication1.Data;
using Company.WebApplication1.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Company.WebApplication1.Pages.Settings.TwoFactorAuth
{
    public class RecoveryCodesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RecoveryCodesModel> _logger;
        private readonly ApplicationDbContext _dbContext;

        public RecoveryCodesModel(UserManager<ApplicationUser> userManager, ILogger<RecoveryCodesModel> logger, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;
        }

        public string[] RecoveryCodes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!user.TwoFactorEnabled)
            {
                //throw new ApplicationException($"Cannot show recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");

                TempData["StatusMessage"] = $"Cannot show recovery codes for user with ID '{user.UserName}' as they do not have 2FA enabled.";
                return RedirectToPage("./Config");
            }

            //var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            //RecoveryCodes = recoveryCodes.ToArray();

            IdentityUserToken<string> userToken = _dbContext.UserTokens
                .Where(token => token.UserId == _userManager.GetUserId(User) &&
                                token.Name == "RecoveryCodes" &&
                                token.LoginProvider == "[AspNetUserStore]")
                .FirstOrDefault();

            RecoveryCodes = userToken.Value.Split(";");

            

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostGenerateNewAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!user.TwoFactorEnabled)
            {
                TempData["StatusMessage"] = $"Cannot show recovery codes for user with ID '{user.UserName}' as they do not have 2FA enabled.";
                return RedirectToPage("./Config");
            }

            await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData["StatusMessage"] = $"You generate new 2FA recovery codes.";

            return RedirectToPage();
        }
    }
}