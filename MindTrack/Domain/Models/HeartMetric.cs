namespace MindTrack.Domain.Models;

public class HeartMetric
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int HeartRate { get; set; }
    public double? Rmssd { get; set; }

    // Relations
    public User? User { get; set; }
}
