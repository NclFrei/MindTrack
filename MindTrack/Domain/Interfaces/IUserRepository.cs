using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> BuscarPorIdAsync(int id);
    Task<bool> VerificaEmailExisteAsync(string email);
    Task<User?> BuscarPorEmailAsync(string email);
    Task<User> CriarAsync(User usuario);
    Task<bool> DeleteAsync(User usuario);
    Task<User> AtualizarAsync(User usuario);
    Task<List<User>> GetAllAsync();

}
