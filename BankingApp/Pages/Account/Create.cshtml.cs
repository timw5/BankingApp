using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BankingApp.Data;
using BankingApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace BankingApp.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly BankingAppContext _db;//context variable for accessing DB


        //****Properties

            //The elements in [square brackets] are Attributes
            //they help us manage errors, and also explain how they interact with the
            //html we are sending to the server, these are very easily injectable by using "@propname"


        //I use a Client side Validation Attribute called PageRemote, There may be a better solution, but
        //I wasn't sure how else to access the database serverside without sending another request,
        //So I used an attribute called PageRemote, which sends a POST request to the server, I created
        //A method called "CheckUsername" that returns a boolean which represents whether
        //the username exists in our database or not, And it won't allow you submit the form until
        //you have supplied a unique username.
        [BindProperty]
        [Required(ErrorMessage = "Username is Required")]
        [PageRemote(ErrorMessage = "Username already exists", AdditionalFields = "__RequestVerificationToken", HttpMethod = "post", PageHandler = "CheckUsername")]
        public string Username { get; set; }

        //password must be >= 8 characters
        [BindProperty]
        [Required(ErrorMessage = "Password is Required"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Confirm Password Field is Required"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords must match")]
        public string PasswordConf { get; set; }

        //*****


        //Constructor for the "Create" object, I should probably rename it to "Register"....
        //it just assigns the dbContext variable to
        //our model this is initialized in Program.cs
        public CreateModel(BankingAppContext db) { _db = db; }


        public IActionResult OnPostkfdkl(int? id)
        {
            var transactions = _db.Transfers.Where(x => x.DepositAccountID == id);
            JsonResult js = new(transactions);
            return js;
            
        }

        //When this page is requested, we render the page
        public IActionResult OnGet()
        {
            return Page();
        }

        //checks if the Username provided in the form is Valid
        public bool IsUsernameValid()
        {
            var x = _db.Users.Where(u => u.Username == Username).ToList();
            if (x.Count == 0)
            {
                return true;
            }
            return false;
        }


        //helper function for OnPostAsync()
        //if any value in the form is null,
        //then the form is not ready to post
        //or if the username is taken
        public bool IsInvalid()
        {
            if (
                Username is null     ||
                Password is null     ||
                PasswordConf is null ||
                Password.Length < 8  ||
                Password != PasswordConf ||
                IsUsernameValid()

               ) { return false; }

            //if(ModelState.IsValid) { return false; } //maybe the code above could be replaced by this?

            return true;
        }





        //When we submit the form, we make sure that no entries in the form
        //are null ( Helper method IsInvalid() ), It is done Asynchronously,
        //it returns a promise of IActionResult(to do an action) once the login information
        //is stored in the database. We then redirect to the Account Page

        // create a new Login object, pass in the username, and password
        // _db.Add(Login) Stages a new entry to be added to the Login table,
        // based on the information saved in the Login object
        // _dv.SaveChangesAsync(); Updates the database with any staged entries asynchronously
        //I set the session variable "ID" to hold the UserID to be used on the account page
        public async Task<IActionResult> OnPostAsync()
        {
            if (this.IsInvalid())
                return Page();         

            Login Login = new(Username, Password);
            _db.Add(Login);
            Models.Account account = new(0, 0, Username, "Checking", Login.ID, Login);
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            if(HttpContext.Session.Get("ID") != null)            
                HttpContext.Session.SetInt32("ID", Login.ID);
       
            return RedirectToPage("/Account/Home");

        }



        //This is the Method used if the username provided on
        //the form is not valid,See my comment above, 
        //It's an asynchronous method, so it returns a
        //Promise (Task) of a JsonResult object,
        //which just ensures that javascript is getting
        //The correct Data type (bool)
        //make this asynchronous
        public JsonResult OnPostCheckUsername()
        {

            var x = _db.Users.Where(x => x.Username == Username).ToList();
            
            if (x.Count == 0 || x is null)
                return new JsonResult(true);

            return new JsonResult(false);

        }

    }
}
