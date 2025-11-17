using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> VerificaEmailExisteAsync(string email);
    Task<User?> BuscarPorEmailAsync(string email);
    Task<User> CriarAsync(User usuario);
}
