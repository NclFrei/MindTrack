using MindTrack.Domain.DTOs.Response;

namespace MindTrack.Application.Service;

public class StressService
{

    public StressResultResponse Calculate(int heartRate, double rmssd)
    {

        double hrNorm = Clamp((heartRate - 40) / (140.0 - 40.0), 0, 1);
        
        double hrvNorm = Clamp((50 - rmssd) / 50.0, 0, 1);

        double scoreF = 0.6 * hrNorm + 0.4 * hrvNorm;
        int score = (int)Math.Round(scoreF * 100);
        string level = score switch
        {
            >= 70 => "Alto",
            >= 40 => "Moderado",
            _ => "Baixo"
        };

        return new StressResultResponse { Score = score, Level = level };
    }

    private static double Clamp(double v, double min, double max) => v < min ? min : v > max ? max : v;
}
