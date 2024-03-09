using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Contracts;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.ValueObjects;

public class Email : ValueObject
{
    internal string Value { get; set; }

    private Email(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<Email> Create(string value, IEmailCheck emailCheck)
    {
        var result = new Result<Email>(new Email(value));
        if (emailCheck.DoesEmailExist(value))
        {
            result.CollectError(ErrorHelper.CreateVeaError("Email already registered.",
                ErrorType.ResourceNotFound));
            return result;
        }

        return Validate(result.Value);
    }

    private static Result<Email> Validate(Email email)
    {
        var result = new Result<Email>(email);

        if (!email.Value.EndsWith("@via.dk", StringComparison.OrdinalIgnoreCase))
        {
            result.CollectError(ErrorHelper.CreateVeaError("Email must end with '@via.dk'.",
                ErrorType.ValidationFailed));
        }

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@via\.dk$";
        if (!Regex.IsMatch(email.Value, emailPattern))
        {
            result.CollectError(ErrorHelper.CreateVeaError("Email is not in a valid format.",
                ErrorType.ValidationFailed));
        }

        if (!email.Value.Contains("@"))
        {
            result.CollectError(ErrorHelper.CreateVeaError("No @ in email.", ErrorType.ValidationFailed));
        }
        else
        {
            ValidateIndividualPartsOfEmail(email, result);
        }

        return result;
    }

    private static void ValidateIndividualPartsOfEmail(Email email, Result<Email> result)
    {
        string text1 = email.Value.Substring(0, email.Value.IndexOf("@", StringComparison.Ordinal));
        if (text1.Length < 3 || text1.Length > 6)
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "The first part of the email must be between 3 and 6 characters long.",
                ErrorType.ValidationFailed));
        }

        string lettersPattern = @"^[A-Za-z]{3,4}$";
        string digitsPattern = @"^\d{6}$";
        if (!(Regex.IsMatch(text1, lettersPattern) || Regex.IsMatch(text1, digitsPattern)))
        {
            result.CollectError(ErrorHelper.CreateVeaError(
                "The first part of the email must be either 3 or 4 letters or 6 digits.",
                ErrorType.ValidationFailed));
        }
    }
}