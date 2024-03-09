using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IVeaEventRepository : IVeaRepository<VeaEvent, VeaEventId>
{
}