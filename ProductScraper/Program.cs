using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProductScarper.DataAccess.Repository;
using ProductScarper.DataAccess.Repository.IRepository;
using ProductScraper.DataAccess;
using ProductScraper.Utility;
using Hangfire;
using Hangfire.PostgreSql;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
/*builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));*/
string? PostgreConnection = Environment.GetEnvironmentVariable("POSTGRESQL");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
    connectionString: PostgreConnection ?? "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=dbname;Integrated Security=true;Pooling=true;"
    ));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>(); ;
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddHostedService<ScopedBackgroundService>();
builder.Services.AddScoped<IProductTrackScopedProcessingService, ProductTrackProcessingService>();
builder.Services.AddHangfire(x =>
    x.UsePostgreSqlStorage(
    connectionString: PostgreConnection ?? "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=dbname;Integrated Security=true;Pooling=true;"
        ));
builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard(); //Will be available under http://localhost:5000/hangfire"

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
