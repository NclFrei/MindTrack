using MindTrack.Domain.Enums;

namespace MindTrack.Domain.DTOs.Request
{
 public class TarefaCreateRequest
 {
 public string Titulo { get; set; } = string.Empty;
 public string? Descricao { get; set; } = string.Empty;
 public DificuldadeEnum Dificuldade { get; set; } = DificuldadeEnum.facil;
 public int Prioridade { get; set; } =1;
 public int UserId { get; set; }
 public int? MetaId { get; set; }
 }
}