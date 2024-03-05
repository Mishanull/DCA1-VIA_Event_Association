using ViaEventAssociation.Core.Domain.Contracts;

namespace UnitTests.FakeServices;

public class FakeCurrentTime : ICurrentTime
{
    public DateTime FakeTime { get; set; }
    public DateTime GetCurrentTime()
    {
        return FakeTime;
    }
}