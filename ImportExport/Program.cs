using ImportExport;
using ImportExport.Models;
using ImportExport.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
 
//
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<DataService>();

// Add Entity Framework Core

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add user registration related services
builder.Services.AddScoped<User>();
builder.Services.AddScoped<Address>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 

app.Run();
