namespace BankingApp.Models
{
    public class Login
    {
        public Login(string username, string hash, string salt)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
        }
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

    }
}
