using Microsoft.EntityFrameworkCore;
using SQLMVC99.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));
//builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // 👈 تغيير المهلة إلى 60 دقيقة
    options.Cookie.HttpOnly = true; // تحسين الأمان
    options.Cookie.IsEssential = true; // يجعل الكوكيز ضروريًا لعمل الجلسة
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// ترتيب الـ Middleware

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
//pattern: "{controller=User}/{action=Index}/{id?}");

//pattern: "{controller=Department}/{action=Index}/{id?}");
//pattern: "{controller=Product}/{action=Index}/{id?}");

pattern: "{controller=User1}/{action=Login}/{id?}");
//pattern: "{controller=Admin}/{action=Index}/{id?}");




app.Run();
