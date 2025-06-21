using System;
using System.ComponentModel.DataAnnotations;

namespace AgentWebApp.Models;

public class DbChangeLog
{
    [Key]
    public int Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Values { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? UserName { get; set; }
}
