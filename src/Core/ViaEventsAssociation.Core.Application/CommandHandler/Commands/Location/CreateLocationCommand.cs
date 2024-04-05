using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Commands.Location;

internal class CreateLocationCommand : Command
{
    internal LocationName LocationName { get; private init; }
    internal CreatorId CreatorId { get; private init; }

    public static Result<CreateLocationCommand> Create(string name, string creatorId)
    {
        var nameResult = LocationName.Create(name);
        var creatorIdResult = TId.FromString<CreatorId>(creatorId);
        var result = new Result<CreateLocationCommand>(null);
        result.CollectFromMultiple(nameResult, creatorIdResult);
        if (result.IsErrorResult())
        {
            return result;
        }

        return new Result<CreateLocationCommand>(new CreateLocationCommand()
        {
            LocationName = nameResult.Value!,
            CreatorId = creatorIdResult.Value!
        });
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return LocationName;
    }
}