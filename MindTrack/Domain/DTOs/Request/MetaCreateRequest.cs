using MindTrack.Domain.Models;

namespace MindTrack.Domain.DTOs.Request;

public class MetaCreateRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public int UserId { get; set; }

}
