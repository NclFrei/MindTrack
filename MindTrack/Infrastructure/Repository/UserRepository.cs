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


    public async Task<bool> VerificaEmailExisteAsync(string email)
    {
        return await _context.User.CountAsync(u => u.Email == email) > 0;
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

}