using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;

public class CreateGuestCommand : Command
{
    internal Email Email { get; private init; }
    internal FirstName FirstName { get; private init; }
    internal LastName LastName { get; private init; }
    internal PictureUrl PictureUrl { get; private init; }

    public static Result<CreateGuestCommand> Create(string email, string firstName, string lastName, string pictureUrl, IEmailCheck emailCheck)
    {
        if (CreateValueObjects(email, firstName, lastName, pictureUrl, emailCheck, out var guestEmailResult, out var firstNameResult, out var lastNameResult, out var pictureUrlResult, out var result)) return result;

        return new Result<CreateGuestCommand>(new CreateGuestCommand()
        {
            Email = guestEmailResult.Value!,
            FirstName = firstNameResult.Value!,
            LastName = lastNameResult.Value!,
            PictureUrl = pictureUrlResult.Value!
        });
    }
    private static bool CreateValueObjects(string email, string firstName, string lastName, string pictureUrl, IEmailCheck emailCheck, out Result<Email> guestEmailResult, out Result<FirstName> firstNameResult, out Result<LastName> lastNameResult,
        out Result<PictureUrl> pictureUrlResult, out Result<CreateGuestCommand> result)
    {

        guestEmailResult = Email.Create(email, emailCheck);
        firstNameResult = FirstName.Create(firstName);
        lastNameResult = LastName.Create(lastName);
        pictureUrlResult = PictureUrl.Create(pictureUrl);
        result = new Result<CreateGuestCommand>(null);
        result.CollectFromMultiple(guestEmailResult, firstNameResult, lastNameResult, pictureUrlResult);
        return result.IsErrorResult();

    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return FirstName;
        yield return LastName;
        yield return PictureUrl;
    }
}