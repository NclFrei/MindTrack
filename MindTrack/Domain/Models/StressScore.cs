using System;

namespace MindTrack.Domain.Models
{
    public class StressScore
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int Score { get; set; }
        public string? Level { get; set; } = string.Empty;
        public int? SourceMetricId { get; set; }

        // Relações
        public User? User { get; set; }

        // Propriedade de navegação referenciada pelo Fluent API em MindTrackContext
        public HeartMetric? SourceMetric { get; set; }
    }
}
