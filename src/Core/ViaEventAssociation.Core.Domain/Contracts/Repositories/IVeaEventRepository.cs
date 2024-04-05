using ViaEventAssociation.Core.Domain.EventAgg;

namespace ViaEventAssociation.Core.Domain.Contracts.Repositories;

public interface IVeaEventRepository : IVeaRepository<VeaEvent, VeaEventId>
{
}