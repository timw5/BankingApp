using Microsoft.EntityFrameworkCore;
using BankingApp.Data;


//create the webapp builder
var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages to the container.
builder.Services.AddRazorPages();

//Configuring our database access with our context variable, using the "Default" connection string
//the connection string is currently stored in appsettings.JSON
builder.Services.AddDbContext<BankingAppContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
