using MindTrack.Application.Service;
using Xunit;

namespace MindTrack.Test.StressServiceTests;

public class Calculate_Should_ReturnScoreAndLevel
{
 [Fact]
 public void Calculate_ReturnsExpectedRange()
 {
 var service = new StressService();
 var result = service.Calculate(80,30);

 Assert.InRange(result.Score,0,100);
 Assert.NotNull(result.Level);
 }
}
