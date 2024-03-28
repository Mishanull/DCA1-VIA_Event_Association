using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandDispatching.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandDispatching.Handlers.Guest;

internal class CreateGuestHandler : ICommandHandler<CreateGuestCommand>
{
    private readonly IGuestRepository _repository;
    private readonly IUnitOfWork _uow;
    internal CreateGuestHandler(IGuestRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
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

        await _uow.SaveChangesAsync();
        return new Result();
    }
}