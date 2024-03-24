using ViaEventAssociation.Core.Domain.Contracts;

namespace ViaEventAssociation.Core.Domain.Services;

public class CurrentTime : ICurrentTime
{
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}