using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface ICreatorRepository : IVeaRepository<Creator, CreatorId>
{
   public Task<Result<Invite>> FindInviteAsync(InviteId id);
}