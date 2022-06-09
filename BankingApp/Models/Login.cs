using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace BankingApp.Models
{
    public class Login
    {
        public Login(string username, string password)
        {
            Username = username;
            var bsalt = GetSalt();
            Hash = HashPass(password, bsalt);
            Salt = SaltToString(bsalt);
        }

        public Login(string username, string hash, string salt)
        {
            Username=username;
            Hash = hash;
            Salt = salt;
        }


        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Hash { get; set; } 

        [Required]
        public string Salt { get; set; } 


        public static string HashPass(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        }

        public static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public static string SaltToString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }




    }
}
