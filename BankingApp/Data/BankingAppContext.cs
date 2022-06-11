using BankingApp.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// This page specifies which tables we want Entity Framework to create
/// To specify what tables we want created, we need to add a class to the 
/// "Models" Folder, and how we create that class, will determine how the
/// table in the database is created, you must supply a constructor!
/// Also if you want a field to be NOT NULL, supply it with the 
/// [Required] Attribute
/// Example:
/// 
///     [Required]
///     public DbSet<Objectname> Tablename { get; set; }
/// 
/// 
/// Once you have supplied the code for your tables here, 
/// 
/// To actually create the tables in the database:
/// go to the search bar, search for "Package Manager Console"
/// Select that, and run the command: 
///     "Add-Migrations MigrationName"
/// then run the command: 
///     "Update-Database"
/// 
/// 
/// </summary>
/// 
namespace BankingApp.Data
{
    public class BankingAppContext : DbContext
    {
        public BankingAppContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Login> Users { get; set; }
        public DbSet<Transfers> Transfers { get; set; }
        public DbSet<Account> Accounts { get; set; }

        //supply more properties here to create more tables
        //*****

        //example:

        //      [Required]
        //      public DbSet<Classname> Tablename { get; set; }

        //*****

    }
}
