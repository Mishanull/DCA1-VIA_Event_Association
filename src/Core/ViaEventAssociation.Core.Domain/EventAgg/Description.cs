﻿using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class Description : ValueObject
{
    internal string Value { get; init; }

    private Description(string value)
    {
        Value = value;
    }
    
    public static Result<Description> Create(string description)
    {
        return Validate(new Description(description));
    }
    public static Result<Description> Create()
    {
        return new Result<Description>(new Description(""));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private static Result<Description> Validate(Description description)
    {
        var errorResult = new Result<Description>(description);
        // the description length is between 0 and 250 (inclusive) characters
        if (description.Value.Length > 250)
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidDescription, new ErrorMessage("Description length must be between 0 and 250 characters")));
        }
        return errorResult;
    }
}