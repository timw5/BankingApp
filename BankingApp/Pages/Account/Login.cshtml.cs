using BankingApp.Data;
using BankingApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly BankingAppContext _db;//database context variable(for accesing the DB

        public LoginModel(BankingAppContext db)//initialized in Program.cs
        {
            
            _db = db;
        }

        //*****Properties

        //The elements in [square brackets] are Attributes
        //they help us manage errors, and also explain how they interact with the
        //html we are sending to the server, these are very easily injectable by using @propname
        

        [BindProperty]
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is Required"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 characters")]
        public string Password { get; set; }

        [BindProperty]
        public string LoginError { get; set; }


        //******

        //When this page is requested, we render the page
        public IActionResult OnGet()
        {
            //HttpContext.Session.Clear();
            if (HttpContext.Session.Get("ID") != null)//checks if I'm already logged in
            {
                return RedirectToPage("/Account/Home");
            }

            return Page();
        }


        //This method checks if the current login is valid, we see if the username provide has a match in the DB,
        //if it does, we return false(login is not valid)
        //if it does not have a match, then the username exists,(Login is valid)
        //so we then get the salt from the row returned from the given username,
        //we get the hash from the returned row,
        //we then hash the password given, and the salt we got from the database I call this testhash
        //then we return the comparison of testhash with the hash value from the database
        //
        public bool IsLoginValid(string username, string password)
        {
            var data = _db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (data is null || data == default)
                return false;

            var salt = Convert.FromBase64String(data.Salt);
            var hash = data.Hash;
            var testhash = Login.HashPass(password, salt);

            return testhash == hash;
        }

        public Login GetLoginInfo(string user, string pass)
        {
            var data = _db.Users.Where(u => u.Username == user).FirstOrDefault();

            if (data is null)
            {
                Login nothing = default!;
                return nothing;
            }
            return data;

        }

        public IActionResult OnGetClearSession()
        {

            HttpContext.Session.Remove("ID");
            HttpContext.Session.Clear();

            return Page();
        }   

        //When we hit submit on our form, we first check to make sure the string's aren't empty,
        //then we pass the username and password provided to the function described above
        //if the login is not valid, we store a message in our "LoginError" property to inject into the webpage
        //and finally, if the passwords match we can redirect to our account page
        //***I currently have it set to /Index because I havent made the Account page yet.
        public async Task<IActionResult> OnPost()
        {
            if(HttpContext.Session.Get("ID") != null)//checks if I'm already logged in
            {
                RedirectToPage("/Account/Home");
            }

            if (Password == String.Empty || Username == String.Empty)
            {
                LoginError = string.Empty;
                return Page();
            }
            else if (IsLoginValid(Username, Password) == false)
            {
                LoginError = "Invalid Username or password";
                return Page();
            }
            else
            {
                var userData = GetLoginInfo(Username, Password);

                HttpContext.Session.SetInt32("ID",userData.ID);////
                Models.Account act;
                LoginError = string.Empty;
                var account = _db.Accounts.Where(u => u.LoginID == userData.ID).FirstOrDefault();
                if (account is null)
                {
                    act = new(0, 0, Username,"Checking", userData.ID, userData);
                    _db.Accounts.Add(act);
                    await _db.SaveChangesAsync();
                }
                //remove this else statement
                else
                    act = account;
               
                return RedirectToPage( "/Account/Home");
            }

        }

 
    }


}
