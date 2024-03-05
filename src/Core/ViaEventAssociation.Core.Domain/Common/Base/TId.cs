using System.Security.Cryptography.X509Certificates;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Base;

public class TId 
{
    internal Guid Id { get; } = Guid.NewGuid();

    protected TId(string id)
    {
        Id = Guid.Parse(id);
    }

    protected TId()
    {
        
    }
    
    public static TId FromString(string id)
    {
        return new TId(id);
    }
}