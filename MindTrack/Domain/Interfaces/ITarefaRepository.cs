using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces
{
 public interface ITarefaRepository
 {
 Task<Tarefa> CreateAsync(Tarefa tarefa);
 Task<List<Tarefa>> GetAllAsync();
 Task<Tarefa?> GetByIdAsync(int id);
 Task<Tarefa?> UpdateAsync(Tarefa tarefa);
 Task<bool> DeleteAsync(int id);
 }
}