using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Data;

namespace MindTrack.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly MindTrackContext _context;

    public UserRepository(MindTrackContext context)
    {
        _context = context;
    }

    public async Task<User?> BuscarPorIdAsync(int id)
    {
        return await _context.User.FindAsync(id);
    }


    public async Task<bool> VerificaEmailExisteAsync(string email)
    {
        return await _context.User.CountAsync(u => u.Email == email) > 0;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.User
            .ToListAsync();
    }

    public async Task<User?> BuscarPorEmailAsync(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CriarAsync(User usuario)
    {
        _context.User.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> DeleteAsync(User usuario)
    {
        _context.User.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<User> AtualizarAsync(User usuario)
    {
        _context.User.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public IQueryable<User> Query()
    {
        return _context.User.AsQueryable();
    }

}