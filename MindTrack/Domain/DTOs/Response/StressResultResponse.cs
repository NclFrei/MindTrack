namespace MindTrack.Domain.DTOs.Response
{
    public class StressResultResponse
    {
        public int Score { get; set; }
        public string Level { get; set; } = string.Empty;
    }
}
