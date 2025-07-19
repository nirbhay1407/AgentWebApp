using AgentWebApp.Data;
using AgentWebApp.Models;
using AgentWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Register ApplicationDbContext with SQL Server
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
// For PostgreSQL, you can use the following line instead:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionPQ") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Register repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register ILogger for ApplicationDbContext
builder.Services.AddLogging();

var app = builder.Build();

app.MapDefaultEndpoints();

// Global exception handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalException");
        logger.LogError(ex, "Unhandled exception occurred while processing request: {Path}", context.Request.Path);

        // Log to DbChangeLog if possible
        try
        {
            var db = context.RequestServices.GetService<ApplicationDbContext>();
            if (db != null)
            {
                db.DbChangeLogs.Add(new DbChangeLog
                {
                    EntityName = "GlobalException",
                    State = "Exception",
                    Values = $"Path: {context.Request.Path}",
                    Exception = ex.ToString(),
                    Timestamp = DateTime.UtcNow
                });
                db.SaveChanges();
            }
        }
        catch { /* Swallow logging errors to avoid recursive exceptions */ }

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
