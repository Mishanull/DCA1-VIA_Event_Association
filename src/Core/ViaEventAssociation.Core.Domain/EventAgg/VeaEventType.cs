using VIAEventsAssociation.Core.Tools.Enumeration;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEventType : Enumeration
{
    public static readonly VeaEventType Public = new(0, "Public");
    public static readonly VeaEventType Private = new(1, "Private");
    
    private VeaEventType(int id, string value) : base(id, value)
    {
    }

    public VeaEventType()
    {
    }
}