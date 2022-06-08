using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingApp.Models;

namespace BankingApp.Data
{
    public class BankingAppContext : DbContext
    {
        public BankingAppContext (DbContextOptions<BankingAppContext> options)
            : base(options)
        {
        }

        public DbSet<BankingApp.Models.Login>? Login { get; set; }
    }
}
