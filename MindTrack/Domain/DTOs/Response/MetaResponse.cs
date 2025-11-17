namespace MindTrack.Domain.DTOs.Response;

public class MetaResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; } 
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; } 
}
