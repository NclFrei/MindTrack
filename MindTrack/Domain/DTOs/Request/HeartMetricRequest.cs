namespace MindTrack.Domain.DTOs.Request;

public class HeartMetricRequest
{
 public int UserId { get; set; }
 public int HeartRate { get; set; }
 public double? Rmssd { get; set; }
 public DateTime? Timestamp { get; set; }
}
