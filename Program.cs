using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PAWUNED_EdgarArias_Proyecto2.Models;
using PAWUNED_EdgarArias_Proyecto2.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(); //soporte para sesiones

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<Proyecto2Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Configuración de PayPal
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));

// Registro del servicio IHttpClientFactory
builder.Services.AddHttpClient();


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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
