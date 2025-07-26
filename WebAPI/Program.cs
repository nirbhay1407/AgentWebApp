using Azure.Monitor.OpenTelemetry.Exporter;
using Hangfire;
using Ioc.Core;
using Ioc.Core.EnumClass;
using Ioc.Core.Helper;
using Ioc.Data.Caches;
using Ioc.Data.CommonMethod;
using Ioc.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.Configuration;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using WebAPI.CustomExceptionMiddleware;
using WebAPI.Mapping;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);



builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"],
            configureApplicationInsightsLoggerOptions: (options) => { }
    );

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Trace", LogLevel.Trace);



/*var connectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'DbContextConnection' not found.");

builder.Services.AddDbContext<IocDbContext>(options =>
    options.UseSqlServer(connectionString));*/

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionPQ") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IocDbContext>(options =>
    options.UseNpgsql(connectionString));


// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IocDbContext>()
    .AddDefaultTokenProviders();
/*builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IocDbContext>();*/

// Create a new OpenTelemetry tracer provider.
// It is important to keep the TracerProvider instance active throughout the process lifetime.
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAzureMonitorTraceExporter(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    })
    .Build();

// Create a new OpenTelemetry meter provider.
// It is important to keep the MetricsProvider instance active throughout the process lifetime.
var metricsProvider = Sdk.CreateMeterProviderBuilder()
    .AddAzureMonitorMetricExporter(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    })
    .Build();

var connectionStrings = builder.Configuration["ApplicationInsights:ConnectionString"];
// Create a new logger factory.
// It is important to keep the LoggerFactory instance active throughout the process lifetime.
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(logging =>
    {
        logging.AddAzureMonitorLogExporter(options =>
        {
            options.ConnectionString = connectionStrings;
        });
    });
});

builder.Services.ExtProgrameMap(builder.Configuration["JWT:ValidAudience"], builder.Configuration["JWT:ValidIssuer"], builder.Configuration["JWT:Secret"]);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.MapServiceSingleton();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});


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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GetUserService>();

/*builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbContextConnection")));
builder.Services.AddHangfireServer();*/

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();

var app = builder.Build();


var loggerFactoryFile = app.Services.GetService<ILoggerFactory>();
loggerFactoryFile.AddFile($"Logs/{Assembly.GetExecutingAssembly().GetName().Name}.log");
var path = Directory.GetCurrentDirectory();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.DocExpansion(DocExpansion.None); });
}
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();
// global error handler.
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
//app.UseHangfireDashboard("/jobs");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
