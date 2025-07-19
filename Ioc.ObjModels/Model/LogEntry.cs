using System;

namespace Ioc.Core.DbModel.Models
{
    public class LogEntry : PublicBaseEntity
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = "Info";
        public string Message { get; set; }
        public string? Exception { get; set; }
        public string? User { get; set; }
        public string? Source { get; set; }
    }
}