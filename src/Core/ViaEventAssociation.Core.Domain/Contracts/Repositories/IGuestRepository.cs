using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IGuestRepository : IVeaRepository<VeaGuest, GuestId>
{
   public Task<Result<Request>> FindRequestAsync(RequestId id);
}