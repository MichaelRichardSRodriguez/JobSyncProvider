using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using JobSyncProvider.DataAccess.Data;
using JobSyncProvider.DataAccess.Services;
using JobSyncProvider.DataAccess.UnitOfWork;
using JobSyncProvider.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add ApplicationDbContext and Get Connection String
builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("JobSyncProviderConnection")));


// Add Razor Pages
builder.Services.AddRazorPages();

// Register services for ASP.NET Core Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();


// Configures the application cookie settings for authentication and authorization.
builder.Services.ConfigureApplicationCookie(option =>
	{
		option.LoginPath = $"/Identity/Account/Login";  // Redirect to login page if not authenticated
		option.LogoutPath = $"/Identity/Account/Logout";  // Redirect to logout page after user logs out
		option.AccessDeniedPath = $"/Identity/Account/AccessDenied";  // Redirect to access denied page for unauthorized users
	}
);



//Register  services lifetime
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ILogoService, LogoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //User Authentication
app.UseAuthorization(); //User Authorization

app.MapRazorPages(); // Map Razor Pages
app.MapControllerRoute(
	name: "default",
	pattern: "{area=Guest}/{controller=Home}/{action=Index}/{id?}");

app.Run();
