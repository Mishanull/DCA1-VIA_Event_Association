
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Base;

public class TId 
{
    public Guid Value { get; private init; }

    protected TId()
    {
        Value = Guid.NewGuid();
    }
    
    public static T Create<T>() where T: TId, new()
    {
        return new T();
    }

    public static Result<T> FromString<T>(string id) where T: TId, new()
    {
        try
        {
            T instance = new ()
            {
                Value = Guid.Parse(id)
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
    
    public static Result<T> FromGuid<T>(Guid id) where T: TId, new()
    {
        T instance = new ()
        {
            Value = id
        };
        
        return new Result<T>(instance);
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }
}