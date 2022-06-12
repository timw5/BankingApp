using Microsoft.EntityFrameworkCore;
using BankingApp.Data;


//create the webapp builder
var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages to the container.
builder.Services.AddRazorPages();

//add memory cache to enable session storage
builder.Services.AddMemoryCache();
builder.Services.AddMvc(); 
            

////***The code below is to configure the cookie policy for the website

//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//    //options.CheckConsentNeeded = context => true;
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//});

//enable session
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(60);//default timeout (log out) after 1 hour
});

//Configuring our database access with our context variable, using the "Default" connection string
//the connection string is currently stored in appsettings.JSON
builder.Services.AddDbContext<BankingAppContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAntiforgery(options =>
{
options.HeaderName = "XSRF-TOKEN";
}

    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//enable session
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
