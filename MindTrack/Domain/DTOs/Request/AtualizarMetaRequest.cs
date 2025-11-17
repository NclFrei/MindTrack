using MindTrack.Domain.Models;

namespace MindTrack.Domain.DTOs.Request;

public class AtualizarMetaRequest
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool? Concluida { get; set; } 

}
