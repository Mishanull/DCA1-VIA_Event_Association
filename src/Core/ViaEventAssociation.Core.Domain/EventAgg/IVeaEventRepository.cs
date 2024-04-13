using ViaEventAssociation.Core.Domain.Common;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public interface IVeaEventRepository : IVeaRepository<VeaEvent, VeaEventId>
{
}