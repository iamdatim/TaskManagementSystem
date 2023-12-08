using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem_BusinessLogic.Logics.Implementations;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_DataSource.Context;
using TaskManagementSystem_DataSource.Entities;
using TaskManagementSystem_DataSource.Repository.Implementation;
using TaskManagementSystem_DataSource.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
      
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepo, TodoRepo>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=AllTask}/{id?}");

app.Run();
