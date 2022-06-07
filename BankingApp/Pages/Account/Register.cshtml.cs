using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankingApp.Pages.Account
{
       public class RegisterModel : PageModel
    {
        [BindProperty]
        public Credentials Credentials { get; set; }

        public void OnGet()
        {
        }
    }
    public class Credentials
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }

}
