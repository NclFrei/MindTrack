namespace MindTrack.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;


    // Relação
    public ICollection<Meta>? Metas { get; set; }

    public void SetPassword(string password)
    {
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool CheckPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }
}
