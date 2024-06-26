using IMS.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IMS.web.Models;
using IMS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using IMS.Infrastructure;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Repository.CRUD;
using IMS.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddDbContext<IMSDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"), e => e.MigrationsAssembly("IMS.web")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddTransient<IRawSqlRepository, RawSqlRepository>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDeniedPath";


});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedingData.InitializeAsync(services);
}



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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
