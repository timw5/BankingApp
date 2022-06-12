using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Models
{
    public class Account
    {
        //constructor for EF
        public Account(int dollars, int cents, string name, string type, int loginID, Login usr)//constructor for a new account
        {
            Dollars = dollars;
            Cents = cents;
            Name = name;
            Type = type;
            LoginID = loginID;
            Login = usr;
        }
        public Account()//default constructor
        {
            Dollars = 0;
            Cents = 0;
            Name = String.Empty;
            Type = "Checking";
            LoginID = 0;
            Login = default!;
        }

        [Key]
        public int ID { get; set; }//primary key
        
        [Required]
        public int Dollars { get; set; }//dollar amount of current balance

        [Required]
        public int Cents { get; set; }//cent amount of current balance

        [Required]
        public string Name { get; set; }//name of account

        [Required]
        public string Type { get; set; }//type of account (savings, investment, checking, etc..)

        [Required]
        [ForeignKey("Login")]
        public int LoginID { get; set; }//foreign key to Users table (Login class)

        [Required]
        public Login Login { get; set; }//User (Login class) that owns this account


        //[Required]
        //public ICollection<Transfers> Transfers { get; set; }






        public string BalanceToString()//print current balance
        {
            return $"${Dollars}.{Cents}";
        }


        //if we add funds and Cents > 100 (1 dollar), we increment dollars,
        //and subtract 100 from cents
        public void AddFunds(int dollars, int cents)//helping to manage the balance
        {
            Dollars += dollars;
            Cents += cents;
            if (Cents >= 100)
            {
                Dollars++;
                Cents -= 100;
            }
        }

        //if we subtract funds, and cents < 0, we decrement dollars,
        //then add a "dollar" value to cents to get the remaining Cent piece of the balance
        public void SubtractFunds(int dollars, int cents)//helping to manage the balance
        {
            Dollars -= dollars;
            Cents -= cents;
            if (Cents < 0)
            {
                Dollars--;
                Cents += 100;
            }
        }

    }

}
