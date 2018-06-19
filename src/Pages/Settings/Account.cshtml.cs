using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WebApplication1.Data;
using Company.WebApplication1.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Company.WebApplication1.Pages.Settings
{
    public class AccountModel : PageModel
    {
        public AccountModel() 
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}