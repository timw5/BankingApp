using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BankingApp.Data;
using BankingApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BankingApp.Pages.Account
{
    public class CreateModel : PageModel
    {
        //private readonly BankingApp.Data.BankingAppContext _context;
        private readonly BankingAppContext _db;

        [BindProperty]
        [Required(ErrorMessage = "Username is Required")]
        [PageRemote(ErrorMessage = "Username already exists", AdditionalFields = "__RequestVerificationToken", HttpMethod = "post", PageHandler = "CheckUsername")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is Required"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Password must be 8 characters")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Confirm Password Field is Required"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords must match")]
        public string PasswordConf { get; set; }


        public CreateModel(BankingAppContext db)
        {
            _db = db;
            
        }


        public IActionResult OnGet()
        {
            return Page();
        }
        
        public bool IsUsernameValid(string username)
        {
            var x = _db.Users.Where(u => u.Username == username).ToList();
            if (x.Count == 0)
            {
                return true;
            }
            return false;
        }

        

        public async Task<IActionResult> OnPostAsync()
        {
            if (Username is null || Password is null || PasswordConf is null)
            {
                return Page();
            }
            if(Password.Length < 8 || Password != PasswordConf)
            {
                return Page();
            }
            if(!IsUsernameValid(Username))
            {
                return Page();                
            }            
            
            Login Login = new(Username, Password);

            _db.Add(Login);
            await _db.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        public JsonResult OnPostCheckUsername()
        {
            var x = _db.Users.Where(u => u.Username == Username).FirstOrDefault();
            if(x is null)
            { 
                return new JsonResult(true);
            }
            return new JsonResult(false);

        }

    }



    //public class RemoteValidateModel : PageModel
    //{
    //    [PageRemote(ErrorMessage = "Username already exists", AdditionalFields = "__RequestVerificationToken",
    //        HttpMethod = "post", PageHandler = "CheckEmail")]
    //    [BindProperty]
    //    public string username { get; set; }
    //    public void OnGet()
    //    {

    //    }
    //    //this method is used to check whether the email is exist or not.
    //    public JsonResult OnPostCheckEmail()
    //    {
    //        //query the database and get all existing Emails or directly check whether the email is exist in the database or not.

    //        var existingEmails = new[] { "jane@test.com", "claire@test.com", "dave@test.com" };
    //        var valid = !existingEmails.Contains(Email);
    //        return new JsonResult(valid);
    //    }
    //}
}
