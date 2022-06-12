using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Models
{
    public class Transfers
    {
        //constructor, DateTime is not provided, but is initialized as the current time when the constructor is called,
        // (when the transfer takes place)
        
        public Transfers(int fromID, int toID, int dollars, int cents, string type, Account deposit, Account Withdrawal)
        {
            WithdrawAccountID = fromID;
            DepositAccountID = toID;
            Dollars = dollars;
            Cents = cents;
            Type = type;
            Time = DateTime.Now;
            DepositAccount = deposit;
            WithdrawAccount = Withdrawal;
            
        }

        public Transfers(int fromID, int toID, int dollars, int cents, string type)
        {
            WithdrawAccountID = fromID;
            DepositAccountID = toID;
            Dollars = dollars;
            Cents = cents;
            Type = type;
            Time = DateTime.Now;
            WithdrawAccount = new();
            DepositAccount = new();
        }
        public Transfers() { }

        public int Id { get; set; }//primary key

        
        [ForeignKey("Account"), Column(Order = 0)]
        public int WithdrawAccountID { get; set; }//FK reference to which account the money is coming from


        public Account WithdrawAccount { get; set; }//which account the money is coming from


        [ForeignKey("Account"), Column(Order = 1)]
        public int DepositAccountID { get; set; }//FK reference to which account the money is being deposited into

        public Account DepositAccount { get; set; }//which account the money is being deposited into

        [Required]
        public int Dollars { get; set; }//dollar amount of transfer

        [Required]
        public int Cents { get; set; }//cents amount of transfer

        [Required]
        public string Type { get; set; }//deposit/withdrawal/transfer

        public DateTime Time { get; set; }//time the transfer took place


        public string AmntToString()//print the amount as a string
        {
            return $"${Dollars}.{Cents}";
        }
    }
}
