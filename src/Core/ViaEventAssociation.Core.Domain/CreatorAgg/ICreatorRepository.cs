using ViaEventAssociation.Core.Domain.Common;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.CreatorAgg;

public interface ICreatorRepository : IVeaRepository<Creator, CreatorId>
{
   public Task<Result<Invite>> FindInviteAsync(InviteId id);
}