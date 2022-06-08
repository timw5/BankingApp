using BankingApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;


namespace BankingApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credentials credentials { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        
        public void OnPost()
        {
            if(credentials.Password is null || credentials.Username is null)
            {
                return;
            }
            var salt = GetSalt();//byte[]
            var hash = Hash(credentials.Password, salt);//string

            Login login = new(credentials.Username, hash, Credentials.SaltToString(salt));

            //now store username, password, salt, and hash in DB, redirect to account page...

        }

        public static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public static string Hash(string password, byte[] salt)
        {
           return Convert.ToBase64String(KeyDerivation.Pbkdf2(
           password: password,
           salt: salt,
           prf: KeyDerivationPrf.HMACSHA256,
           iterationCount: 100000,
           numBytesRequested: 256 / 8));

        }
        
 
    }
    public class Credentials
    {
        
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }


        public static string SaltToString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
    }

}
