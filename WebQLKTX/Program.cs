using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using Microsoft.AspNetCore.Identity.UI;
using QuanLyKTX.Areas.Admin.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với chuỗi kết nối
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuanLyKTXDatabase"))); // Updated to match appsettings.json

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>(); // Use AppDbContext instead of ApplicationDbContext

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Chuyển hướng đến trang đăng nhập mặc định của Identity
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Nếu không đủ quyền
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Thêm vai trò Admin và User nếu chưa tồn tại
    var roles = new[] { SD.Admin, SD.User };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Đảm bảo người dùng mặc định có vai trò User
    var user = await userManager.FindByEmailAsync("user@example.com");
    if (user != null && !await userManager.IsInRoleAsync(user, "User"))
    {
        await userManager.AddToRoleAsync(user, "User");
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication(); // Quan trọng! Bật xác thực trước khi kiểm tra quyền truy cập
    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
