﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankingApp.Data;
using BankingApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Web;


namespace BankingApp.Pages.Account
{
    //[ValidateAntiForgeryToken]

    public class AccountModel : PageModel
    {
        //database context variable
        private readonly BankingAppContext _db;

        [BindProperty]
        public int count { get; set; } = 0;

        //list of accounts attached to the current user
        [BindProperty, Required]
        public ICollection<Models.Account> _Account { get; set; } = default!;

        //user currently logged in
        [BindProperty]
        public Login _User { get; set; }

        [BindProperty]
        public int ID { get; set; }

        [BindProperty]
        [Required]
        //property for managing the "Add New Account" button, I dont want it
        //to be displayed if there are alread 3 accounts.
        public string hidden {get; set;} 



        //constructor for this model
        public AccountModel(BankingApp.Data.BankingAppContext context)
        {
            _User = default!;
            _db = context;
        }


        //This is the handler for the Add New Account button,
        //I use the Session Variable "ID" to get the user
        //that owns this account
        public IActionResult OnPostAddNewAccountAsync([FromBody]string? acntType)
        {
            if (HttpContext.Session.Get("ID") != null)
            {
                this.ID = (int)HttpContext.Session.GetInt32("ID");
            }
            var user = _db.Users.Where(x => x.ID == this.ID).FirstOrDefaultAsync().Result;
            
            if (user is not null && acntType is not null)
            {
                _User = user;
                _Account = _User.Accounts;
                Models.Account account = new(0, 0, user.Username, acntType, user.ID, user);
                _User.Accounts.Add(account);
                _Account.Add(account);
                _db.Accounts.Add(account);
                _db.SaveChangesAsync().Wait();
            }
           
            
            
            return Page();
        }
        




        //all users start out with a default "Checking" account

        //I use the session variable to get the UserID
        //I asynchronously get that user from the db
        //I then get the Accounts owned by that user
        //I then assign the user property for this model
        //I then check to see which "State" the model is in,
        //if the users account list is empty, then add a default checking account
            //to the database, and the user
        //if user account list is null, then initialize it, and assign the accounts
            //that I got from the database to the user accounts, and the account property
            //of this model
        //and if we pass all of that, then i simply assign the user account
            //list to be the accounts I got from the database
            //and assign the account property of this model to be the
            //user account list
        //I also toggle the "hidden" property to determine whether I should allow
            //the user to add a new account or not (Max 3 accounts) (for now..)
        //and finally if we fail the first check (if user is not null)
            //we arent logged in, so I redirect the user to the login page
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.Get("ID") != null)
            {
                this.ID = (int)HttpContext.Session.GetInt32("ID");
            }

            var user = await _db.Users.Where(x=>x.ID == this.ID).FirstOrDefaultAsync();

            if (user is not null)
            {
                _User = user;
                var accounts = await _db.Accounts.Where(x=>x.LoginID == _User.ID).ToListAsync();
                if (accounts is null || accounts.Count == 0)
                {
                    
                    _User.Accounts = new List<Models.Account>();
                    _Account = _User.Accounts;
                    Models.Account act = new(0, 0, _User.Username, "Checking", this.ID, _User);
                    _Account.Add(act);
                    _db.Accounts.Add(act);
                    await _db.SaveChangesAsync();
                }
                else if(_User.Accounts is null)
                {
                    _User.Accounts = new List<Models.Account>();
                    _User.Accounts = accounts;
                    _Account = accounts;
                }
                else
                {
                    _User.Accounts = accounts;
                    _Account = _User.Accounts;
                }
                this.hidden = "inline-block";

                if (_Account.Count > 2)
                {
                    this.hidden = "none";
                }
                return Page();
            }
            return RedirectToPage("/Account/Login");
        }



        
        public JsonResult OnGetTransactions(string type)
        {
            JsonResult js = new JsonResult("Hello World");
            var x = Request.Query["val"];
            JsonContent jc;
            js.Value = x;
            js.ContentType = "application/json";
            js.StatusCode = 200;
            
            


            return js;





        }

    }
}
