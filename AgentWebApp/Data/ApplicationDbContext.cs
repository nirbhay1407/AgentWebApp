using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgentWebApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AgentWebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
        : base(options)
    {
        _logger = logger;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<DbChangeLog> DbChangeLogs { get; set; }

    public override int SaveChanges()
    {
        try
        {
            LogEntityChanges();
            return base.SaveChanges();
        }
        catch (Exception ex)
        {
            LogEntityChanges(ex);
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            LogEntityChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            LogEntityChanges(ex);
            throw;
        }
    }

    private void LogEntityChanges(Exception? ex = null)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;
            var state = entry.State.ToString();
            var values = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
            var logMessage = $"[DB LOG] Entity: {entityName}, State: {state}, Values: {{ {string.Join(", ", values.Select(kv => kv.Key + ": '" + kv.Value + "'"))} }}";
            _logger.LogInformation(logMessage);
            DbChangeLogs.Add(new DbChangeLog
            {
                EntityName = entityName,
                State = state,
                Values = string.Join(", ", values.Select(kv => kv.Key + ": '" + kv.Value + "'")),
                Exception = ex?.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
