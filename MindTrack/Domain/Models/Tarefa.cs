using MindTrack.Domain.Enums;

namespace MindTrack.Domain.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public DificuldadeEnum Dificuldade { get; set; } = DificuldadeEnum.facil;
        public int Prioridade { get; set; } = 1;

        // Relações
        public int UserId { get; set; }
        public User? User { get; set; }

        public int? MetaId { get; set; }
        public Meta? Meta { get; set; }
    }
}
