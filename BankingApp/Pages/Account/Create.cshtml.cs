using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BankingApp.Data;
using BankingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly BankingApp.Data.BankingAppContext _context;

        public CreateModel(BankingApp.Data.BankingAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NewUser newusr { get; set; }

 

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (newusr.Username is null || newusr.Password is null || newusr.PasswordConf is null)
            {
                return Page();
            }
            var salt = LoginModel.GetSalt();
            var hash = LoginModel.Hash(newusr.Password, salt);
            Login Login = new(newusr.Username, hash, Credentials.SaltToString(salt));

            _context.Login.Add(Login);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class NewUser : Credentials
    {
 
        [Required, DataType(DataType.Password)]
        public string PasswordConf { get; set; }

    }
}
