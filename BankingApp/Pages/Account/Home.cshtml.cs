using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankingApp.Data;
using BankingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Pages.Account
{
    public class AccountModel : PageModel
    {
        private readonly BankingApp.Data.BankingAppContext _context;

        [BindProperty, Required]
        public ICollection< Models.Account> _Account { get; set; } = default!;

        public Login User { get; set; }
        



        public AccountModel(BankingApp.Data.BankingAppContext context)
        {
            _context = context;
        }


        public void OnGetLoggedIn(int id)
        {

            _Account = new List<Models.Account>();
            var x = _context.Accounts.Where(x => x.ID == id).FirstOrDefaultAsync().Result;
            if (x != null)
                _Account.Add(x);
            else RedirectToPage("/Login");
            return;
        }


        public async Task<IActionResult> OnGetAsync(int ID)
        {
            var user = _context.Users.Where(x=>x.ID == ID).FirstOrDefaultAsync().Result;
            if (user is not null)
            {
                User = user;
                var accounts = _context.Accounts.Where(x=>x.LoginID == User.ID).ToList();
                if (accounts is null || accounts.Count == 0)
                {
                    User.Accounts = new List<Models.Account>();
                    _Account = User.Accounts;
                    Models.Account act = new Models.Account(0, 0, User.Username, "Checking", User.ID, User);
                    _Account.Add(act);
                    _context.Accounts.Add(act);
                    await _context.SaveChangesAsync();
                }
                else if(User.Accounts is null)
                {
                    User.Accounts = new List<Models.Account>();
                    User.Accounts = accounts;
                    _Account = accounts;
                }
                else
                {
                    User.Accounts = accounts;
                    _Account = User.Accounts;
                }
                return Page();
            }
            return RedirectToPage("/Account/Login");
        }
    }
}
