using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models
{
    public class Transfers
    {
        //constructor, DateTime is not provided, but is initialized as the current time when the constructor is called,
        // (when the transfer takes place)
        
        public Transfers(int fromID, int toID, int dollars, int cents, string type)
        {
            FromID = fromID;
            ToID = toID;
            Dollars = dollars;
            Cents = cents;
            Type = type;
            Time = DateTime.Now;
        }

        public int Id { get; set; }//primary key

        [Required]
        public int FromID { get; set; }//which account the xfer came from

        [Required]
        public int ToID { get; set; }//which account the xfer is going to

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
