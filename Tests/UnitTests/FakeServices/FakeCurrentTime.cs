using ViaEventAssociation.Core.Domain.Contracts;

namespace UnitTests.FakeServices;

public class FakeCurrentTime(DateTime fakeTime) : ICurrentTime
{
    public DateTime FakeTime { get; set; } = fakeTime;

    public FakeCurrentTime() : this(DateTime.Now)
    {
    }

    public DateTime GetCurrentTime()
    {
        return FakeTime;
    }
}