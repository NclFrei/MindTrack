using MindTrack.Domain.Enums;

namespace MindTrack.Domain.DTOs.Response
{
 public class TarefaResponse
 {
 public int Id { get; set; }
 public string Titulo { get; set; } = string.Empty;
 public string? Descricao { get; set; } = string.Empty;
 public DificuldadeEnum Dificuldade { get; set; }
 public int Prioridade { get; set; }
 public int? MetaId { get; set; }
 }
}