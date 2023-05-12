using BussinessLayer.Models;
using BussinessLayer.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<CompetitionService>();
builder.Services.AddSingleton<CompetetiveClassService>();
builder.Services.AddSingleton<CoupleService>();

var app = builder.Build();
Seed();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{name?}");

app.Run();


void Seed()
{
    var _competetiveClassService = app.Services.GetService<CompetetiveClassService>();
    var _coupleService = app.Services.GetService<CoupleService>();

    _competetiveClassService.Create(new CompetetiveClass()
    {
        Name = "Set up class",
        JudgesCount = 10
    });

    _coupleService.Create(new Couple()
    {
        Name = "Set up couple",
        CompetetiveClass = "Set up"
    }, 10);
}