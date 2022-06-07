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


}
