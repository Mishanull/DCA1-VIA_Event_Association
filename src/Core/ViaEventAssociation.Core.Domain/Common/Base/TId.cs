
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Base;

public class TId 
{
    internal Guid Id { get; private set; }

    protected TId()
    {
        Id = Guid.NewGuid();
    }

    public static Result<T> FromString<T>(string id) where T: TId, new()
    {
        try
        {
            T instance = new ()
            {
                Id = Guid.Parse(id)
            };
            
            return new Result<T>(instance);
        }
        catch (FormatException e)
        {
            var errorResult = new Result<T>(null);
            errorResult.CollectError(ErrorHelper.CreateVeaError(e.Message, ErrorType.ValidationFailed));
            return errorResult;
        }
    }
    
    public override string ToString()
    {
        return Id.ToString();
    }
}