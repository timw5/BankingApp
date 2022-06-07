using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credentials credentials { get; set; }
       
        public void OnGet()
        {
        }
    }
    public class Credentials
    {
        
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }

}
