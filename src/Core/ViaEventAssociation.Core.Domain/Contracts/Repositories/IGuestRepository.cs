using ViaEventAssociation.Core.Domain.GuestAgg.Guest;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IGuestRepository : IVeaRepository<VeaGuest, GuestId>
{
    
}