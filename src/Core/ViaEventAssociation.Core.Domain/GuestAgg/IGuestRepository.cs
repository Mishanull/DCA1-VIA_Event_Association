using ViaEventAssociation.Core.Domain.Common;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg;

public interface IGuestRepository : IVeaRepository<VeaGuest, GuestId>
{
   public Task<Result<RequestEntity.Request>> FindRequestAsync(RequestId id);
}