using BankingApp.Data;
using BankingApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace BankingApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly BankingAppContext _db;

        public LoginModel(BankingAppContext db)
        {
            _db = db;
        }

        [BindProperty]
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is Required"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 characters")]
        public string Password { get; set; }

        [BindProperty]
        public string LoginError { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }

        public bool IsLoginValid(string username, string password)
        {
            var data = _db.Users.Where(u => u.Username == username).FirstOrDefault();
            if(data is null || data == default)
                return false;

            var salt = Convert.FromBase64String(data.Salt);
            var hash = data.Hash;
            var testhash = Login.HashPass(password, salt);
            return testhash == hash;
        }



        public IActionResult OnPost()
        {
            if (Password == String.Empty || Username == String.Empty)
            {
                LoginError = string.Empty;
                return Page();
            }
            else if (IsLoginValid(Username, Password) == false)
            {
                LoginError = "Invalid Username or password";
                return Page();
            }
            else
            {
                LoginError = string.Empty;
                return RedirectToPage("/Index");
            }

        }

 
    }


}
