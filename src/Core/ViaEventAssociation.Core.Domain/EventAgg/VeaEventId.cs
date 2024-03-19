using ViaEventAssociation.Core.Domain.Common.Base;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEventId : TId
{
    
    public VeaEventId()
    {
        
    }
    
    internal VeaEventId(string id) : base(id)
    {
    }
}
