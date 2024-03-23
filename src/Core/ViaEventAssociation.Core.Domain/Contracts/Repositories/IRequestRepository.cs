using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IRequestRepository : IVeaRepository<Request, RequestId> 
{
    
}