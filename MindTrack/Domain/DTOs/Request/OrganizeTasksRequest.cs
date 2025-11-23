namespace MindTrack.Domain.DTOs.Request;

public class OrganizeTasksRequest
{
 public int UserId { get; set; }
 public float StressScore { get; set; }
 public int? MetaId { get; set; }
}
