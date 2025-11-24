namespace MindTrack.Domain.ML
{
    public class TaskRankingInput
    {
        public float Prioridade { get; set; }
        public float Dificuldade { get; set; }
        public float MetaId { get; set; }
        public float StressScore { get; set; }


        public float Label { get; set; }
    }
}
