using CodeLearn.Core.Convertors;
using CodeLearn.Core.Services;
using CodeLearn.Core.Services.Interfaces;
using CodeLearn.DataLayer.Context;
using CodeLearn.DataLayer.Entities.User;
using CodeLearn.Web.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

//tanzimate ehraze hoviyat
#region Authentication 
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme=CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme=CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/Login";   //agar kaarbar be paneli bexahad beravad k dastresi nadarad be login redirect mishavad
    option.LogoutPath = "/Logout";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(43200);  //engezaye be xater besepar
});
#endregion  

#region DbContext
string connString = builder.Configuration.GetConnectionString("CodeLearnConnection");

builder.Services.AddDbContext<CodeLearnContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodeLearnConnection"));

});
#endregion

#region Ioc
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IOrderService, OrderService>();
#endregion

var app = builder.Build();


// configure the http request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAuthentication();      //estefade az ehraze hoviyat

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/hi", () => "Hello!");

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
