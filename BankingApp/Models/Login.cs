using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace BankingApp.Models
{
    public class Login
    {
        public Login(string username, string password)//constructor for creating a new account
        {
            Username = username;
            var bsalt = GetSalt();
            Hash = HashPass(password, bsalt);
            Salt = SaltToString(bsalt);
        }

        public Login(string username, string hash, string salt)//constructor for the Context (Entity Framework)
        {
            Username = username;
            Hash = hash;
            Salt = salt;
            Accounts = new List<Account>();
        }


        public int ID { get; set; }//primary key

        [Required]
        public string Username { get; set; }//nothing fancy

        [Required]
        public string Hash { get; set; }//also stored as a base64 string

        [Required]
        public string Salt { get; set; }//stored as a base64 string, but must be a byte[] to use it for hashing

        public ICollection<Account> Accounts { get; set; }//list of accounts owned by the user



        //this is a recommended hashing algorithm from microsoft, although it requires a byte array for the salt,
        //sql will store this value as a string(Base64 string), I store it in this object as a string, but when I 
        //am verifying the hash when the user logs in, I cast the string back to a byte[] using FromBase64String() method.
        public static string HashPass(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        }


        //generate random salt as a byte[]
        public static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        //casting the byte[] to a base64 string
        public static string SaltToString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }




    }
}
