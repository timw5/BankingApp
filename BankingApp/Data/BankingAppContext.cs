using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.Data
{
    public class BankingAppContext : DbContext
    {
        public BankingAppContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Login> Users { get; set; }

    }
}
