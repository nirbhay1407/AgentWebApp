using Hangfire;
using Ioc.Core;
using Ioc.Core.EnumClass;
using Ioc.Core.Helper;
using Ioc.Data.Caches;
using Ioc.Data.Data;
using IocContainer.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionPQ") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IocDbContext>(options =>
    options.UseNpgsql(connectionString));

//added connnection string to the External logget Ectensioon class
//builder.Services.AddSingleton<ExceptionExtensions>(new ExceptionExtensions(connectionString));
ExceptionExtensions.SetConnectionString(connectionString);
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddDefaultUI()
    .AddEntityFrameworkStores<IocDbContext>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.MapServiceSingleton();
//builder.Services.MapServiceSingleton();

//builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddMvcCore();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.Configure<CacheConfiguration>(builder.Configuration.GetSection("CacheConfiguration"));
//For In-Memory Caching
builder.Services.AddMemoryCache();
builder.Services.AddTransient<MemoryCacheService>();
builder.Services.AddTransient<RedisCacheService>();
builder.Services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
{
    switch (key)
    {
        case CacheTech.Memory:
            return serviceProvider.GetService<MemoryCacheService>();
        case CacheTech.Redis:
            return serviceProvider.GetService<RedisCacheService>();
        default:
            return serviceProvider.GetService<MemoryCacheService>();
    }
});

//builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbContextConnection")));
//builder.Services.AddHangfireServer();
builder.Services.AddSession();
builder.Services.AddMvc();
//builder.Services.AddHostedService<AccountRemovalService>();

builder.Services.AddControllers();
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

app.MapControllers();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
