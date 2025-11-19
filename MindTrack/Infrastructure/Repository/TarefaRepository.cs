using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Data;

namespace MindTrack.Infrastructure.Repository;

public class TarefaRepository : ITarefaRepository
{
     private readonly MindTrackContext _context;

     public TarefaRepository(MindTrackContext context)
     {
        _context = context;
     }

     public async Task<Tarefa> CreateAsync(Tarefa tarefa)
     {
         _context.Tarefas.Add(tarefa);
         await _context.SaveChangesAsync();
         return tarefa;
     }

     public async Task<List<Tarefa>> GetAllAsync()
     {
         return await _context.Tarefas
         .ToListAsync();
     }

     public async Task<Tarefa?> GetByIdAsync(int id)
     {
        return await _context.Tarefas.FirstOrDefaultAsync(a => a.Id == id);
     }

     public async Task<Tarefa?> UpdateAsync(Tarefa tarefa)
     {
         _context.Tarefas.Update(tarefa);
         await _context.SaveChangesAsync();
         return tarefa;
     }

     public async Task<bool> DeleteAsync(int id)
     {
         var tarefa = await _context.Tarefas.FindAsync(id);
         if (tarefa == null) return false;

         _context.Tarefas.Remove(tarefa);
         await _context.SaveChangesAsync();
         return true;
     }
}