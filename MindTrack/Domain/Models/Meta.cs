namespace MindTrack.Domain.Models;

public class Meta
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; } = false;

    // Relação
    public int UserId { get; set; }
    public User? User { get; set; }

    // Relação com tarefas
    public ICollection<Tarefa>? Tarefas { get; set; } = new List<Tarefa>();

}
