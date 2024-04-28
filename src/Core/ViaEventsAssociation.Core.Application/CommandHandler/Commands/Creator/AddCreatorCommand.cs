using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Creator;

public class AddCreatorCommand : Command
{
    internal Email Email { get; private init; }
    
    public static Result<AddCreatorCommand> Create(string email, IEmailCheck emailCheck)
    {
        var emailResult = Email.Create(email, emailCheck);
        if (emailResult.IsErrorResult())
        {
            return ResultHelper.CreateErrorResultWithSingleError<AddCreatorCommand>(
                ErrorHelper.CreateVeaError("Email invalid.", ErrorType.ValidationFailed), null);
        }

        return new Result<AddCreatorCommand>(new AddCreatorCommand()
        {
            Email = emailResult.Value!
        });
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
    }
}