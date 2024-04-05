using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;

internal class CreateGuestHandler : ICommandHandler<CreateGuestCommand>
{
    private readonly IGuestRepository _repository;
    internal CreateGuestHandler(IGuestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> HandleAsync(CreateGuestCommand command)
    {
        var guestCreateResult = VeaGuest.Create(command.Email, command.FirstName, command.LastName, command.PictureUrl);
        if (guestCreateResult.IsErrorResult())
        {
            return guestCreateResult;
        }

        var repositoryResult = await _repository.AddAsync(guestCreateResult.Value!);
        if (repositoryResult.IsErrorResult())
        {
            return repositoryResult;
        }

        return new Result();
    }
}