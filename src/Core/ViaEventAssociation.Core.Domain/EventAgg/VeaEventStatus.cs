using VIAEventsAssociation.Core.Tools.Enumeration;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEventStatus : Enumeration
{
    public static readonly VeaEventStatus Draft = new(1, "Draft");
    public static readonly VeaEventStatus Ready = new(2, "Ready");
    public static readonly VeaEventStatus Active = new(3, "Active");
    public static readonly VeaEventStatus Cancelled = new(4, "Cancelled");
    
    private VeaEventStatus(int id, string value) : base(id, value)
    {
    }
}