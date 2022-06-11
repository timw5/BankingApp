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
        private readonly BankingAppContext _db;

        public TransactionsModel(BankingAppContext context)
        {
            _db = context;
        }

        public IList<Transfers> Transfers { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_db.Transfers != null)
            {
                Transfers = await _db.Transfers.ToListAsync();
            }
        }
    }
}
