namespace MindTrack.Domain.DTOs.Response;

public class StressScoreResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public int Score { get; set; }
    public string Level { get; set; } = string.Empty;
    public int? SourceMetricId { get; set; }
}
