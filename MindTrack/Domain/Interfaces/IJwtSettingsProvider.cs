namespace MindTrack.Domain.Interfaces;

public interface IJwtSettingsProvider
{
    string SecretKey { get; }
}
