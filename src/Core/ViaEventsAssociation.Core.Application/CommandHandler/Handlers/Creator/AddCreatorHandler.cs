using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Creator;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventsAssociation.Core.Application.CommandHandler.Handlers.Creator;

public class AddCreatorHandler : ICommandHandler<AddCreatorCommand>
{
    private readonly ICreatorRepository _creatorRepository;

    public AddCreatorHandler(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
    }

    public async Task<Result> HandleAsync(AddCreatorCommand command)
    {
        var creator = ViaEventAssociation.Core.Domain.CreatorAgg.Creator.Create(command.Email);
        if (creator.IsErrorResult())
        {
            return creator;
        }

        var repoResult = await _creatorRepository.AddAsync(creator.Value!);
        return repoResult.IsErrorResult() ? repoResult : new Result();
    }
}