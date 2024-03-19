using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.Services;

namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IGuestRepository : IVeaRepository<VeaGuest, GuestId>
{
    
}