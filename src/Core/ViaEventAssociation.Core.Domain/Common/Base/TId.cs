
namespace ViaEventAssociation.Core.Domain.Common.Base;

public class TId 
{
    internal Guid Id { get; } = Guid.NewGuid();

    public TId(string id)
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