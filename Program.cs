using EducaPlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<EducaPlusIdentityContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("EducaPlus")
    )
);

// Autenticação e perfis
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // Ambiente local de desenvolvimento
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<EducaPlusIdentityContext>();

builder.Services.AddRazorPages();




// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EducaPlusContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EducaPlus")
    )
);


var app = builder.Build();


await IdentityInitializer.InitializeAsync(
    app.Services,
    app.Configuration
);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
app.Run();
