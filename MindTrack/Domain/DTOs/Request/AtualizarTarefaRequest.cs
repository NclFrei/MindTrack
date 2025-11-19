using MindTrack.Domain.Enums;

namespace MindTrack.Domain.DTOs.Request
{
 public class AtualizarTarefaRequest
 {
 public string? Titulo { get; set; }
 public string? Descricao { get; set; }
 public DificuldadeEnum? Dificuldade { get; set; }
 public int? Prioridade { get; set; }
 public int? MetaId { get; set; }
 }
}