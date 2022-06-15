using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankingApp.Data;
using BankingApp.Models;

namespace BankingApp.Pages.Account
{
    public class TransactionsModel : PageModel
    {
        private readonly BankingApp.Data.BankingAppContext _context;

        [BindProperty]
        public string test { get; set; } = "test";

        public TransactionsModel(BankingApp.Data.BankingAppContext context)
        {
            _context = context;
        }

      public Transfers Transfers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.Get("ID") is null)//we arent logged in
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        //public IActionResult OnGetChange()
        //{

        //}
    }
}
