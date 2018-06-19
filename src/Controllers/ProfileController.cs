using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Company.WebApplication1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Company.WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        private ApplicationUser _currentUser;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var user = _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                    }

                    _currentUser = user.Result;
                }
                return _currentUser;
            }
        }

        public bool IsEmailConfirmed
        {
            get
            {
                return _userManager.IsEmailConfirmedAsync(CurrentUser).Result;
            }
        }

        public bool IsHasPassword
        {
            get
            {
                return _userManager.HasPasswordAsync(CurrentUser).Result;
            }
        }

        public class ChangePasswordInput
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordInput model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(CurrentUser, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _signInManager.SignInAsync(CurrentUser, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");

            return Ok("Your password has been changed.");
        }

        public class SetPasswordInput
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordInput model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(CurrentUser, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _signInManager.SignInAsync(CurrentUser, isPersistent: false);
            _logger.LogInformation("User set password successfully.");

            return Ok("Your password has been set.");
        }
    }
}