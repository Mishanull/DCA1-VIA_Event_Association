using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg.Guest;

public class PictureUrl : ValueObject
{
    
    internal string Value { get; }

    public PictureUrl()
    {
        Value = "";
    }

    private PictureUrl(string value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<PictureUrl> Create(string value)
    {
        return Validate(new PictureUrl(value));
    }

    private static Result<PictureUrl> Validate(PictureUrl pictureUrl)
    {
        var result = new Result<PictureUrl>(pictureUrl);
        string pattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?.\.(jpg|jpeg|png|gif|bmp)$";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(pictureUrl.Value) ? result : CollectAndReturn(result);
    }

    private static Result<PictureUrl> CollectAndReturn( Result<PictureUrl> result)
    {
        result.CollectError(ErrorHelper.CreateVeaError("Invalid picture url.", ErrorType.ValidationFailed));
        return result;
    } 
}