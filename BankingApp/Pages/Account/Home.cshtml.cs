using System;
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
using Newtonsoft.Json;

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
        public List<Models.Account> _Account { get; set; } = default!;

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


        public async Task<IActionResult> OnPostTransferFunds([FromBody] dynamic? data)
        {

            if (data is not null)
            {
                var json = JsonConvert.DeserializeObject<IDictionary<string, string>>(data.ToString());
                //data is now a dictionary
                if (HttpContext.Session.Get("ID") != null)
                {
                    var ID = (int)HttpContext.Session.GetInt32("ID");

                    int Toacntid = int.Parse(json["toID"]);
                    int Fromacntid = int.Parse(json["fromID"]);
                    int dollars = int.Parse(json["dollars"]);
                    int cents = int.Parse(json["cents"]);

                    var Toacnt = await _db.Accounts.Where(x => x.ID == Toacntid).FirstOrDefaultAsync();
                    var Fromacnt = await _db.Accounts.Where(x => x.ID == Fromacntid).FirstOrDefaultAsync();

                    if (Toacnt is not null && Fromacnt is not null)
                    {
                        Transfers t = new(Fromacntid, Toacntid, dollars, cents, "Transfer", Toacnt, Fromacnt);
                        if (Fromacnt.Dollars <= 1 || Fromacnt.Dollars <= dollars)
                        {
                            return StatusCode(500, "Not enough funds to complete transfer");
                        }
                        Fromacnt.SubtractFunds(dollars, cents);
                        Toacnt.AddFunds(dollars, cents);
                        Fromacnt.Withdrawals.Add(t);
                        Toacnt.Deposits.Add(t);
                        _db.Transfers.Add(t);
                        await Task.Run(() => _db.SaveChangesAsync());
                    }
                }
            }
            return Page();
        }


        //This is the handler for the Add New Account button,
        //I use the Session Variable "ID" to get the user
        //that owns this account
        //I query the database for the user information
        //I then add a new account with the name provided from the javascript ajax POST call
        //that value is received to this method a
        ///s "acntType"   
        public async Task<IActionResult> OnPostAddNewAccount([FromBody]string? acntType)//[FromBody] attribute specifies the value is coming from a POST request
        {

            if (HttpContext.Session.Get("ID") != null)
            {
                this.ID = (int)HttpContext.Session.GetInt32("ID");

                var user = await _db.Users.Where(x => x.ID == this.ID).FirstOrDefaultAsync();

                if (user is not null && acntType is not null)
                {
                    _User = user;
                    _Account = (List<Models.Account>)_User.Accounts;
                    Models.Account account = new(0, 0, user.Username, acntType, user.ID, user);
                    _User.Accounts.Add(account);
                    user.Accounts.Add(account);
                    _Account.Add(account);
                    _db.Accounts.Add(account);
                    await _db.SaveChangesAsync();
                }
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostAddFunds([FromBody]dynamic? data)
        {
            if (data is not null)
            {
                var json = JsonConvert.DeserializeObject<IDictionary<string, string>>(data.ToString());
                //json is now a dictionary containing the json data sent from javascript

                int acntid = int.Parse(json["ID"]);
                int dollars = int.Parse(json["dollars"]);
                int cents = int.Parse(json["cents"]);

                if (HttpContext.Session.Get("ID") != null)
                {
                    var id = HttpContext.Session.GetInt32("ID");
                    var acnt =  await _db.Accounts.Where(x => x.ID == acntid).FirstAsync();
                    if (acnt is not null)
                    {
                        Transfers t = new Transfers(acntid, acntid, dollars, cents, "Deposit", acnt, acnt);
                        acnt.Deposits.Add(t);
                        acnt.AddFunds(dollars, cents);
                        _db.Transfers.Add(t);
                        await _db.SaveChangesAsync();
                    }
                }
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
                    _Account = (List<Models.Account>)_User.Accounts;
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
                    _Account = (List<Models.Account>)_User.Accounts;
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
    }
}