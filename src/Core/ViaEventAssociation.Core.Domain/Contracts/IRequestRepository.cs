using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;

namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IRequestRepository : IVeaRepository<Request, RequestId> 
{
    
}