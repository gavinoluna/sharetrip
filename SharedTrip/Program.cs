using Microsoft.EntityFrameworkCore;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["DefaultConnection"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new Cloudinary(builder.Configuration["CloudinaryString"]));

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();