using ViaEventAssociation.Core.Domain.Common.Base;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEvent (VeaEventId id) : AggregateRoot(id)
{
    public VeaEventId VeaEventId { get; }
    internal Title Title { get; set; }
    internal Description Description { get; }
    internal VeaEventType VeaEventType { get; }
    internal MaxGuests MaxGuests { get; }
    internal VeaEventStatus VeaEventStatus { get; set; } = VeaEventStatus.Draft;
    internal TimeSpan FromTo { get; }
    // internal CreatorId CreatorId { get; }
    // internal LocationId LocationId { get; }
    // internal List<GuestId> Participants { get; }

    internal Result UpdateTitle(Title title)
    {
        var errorResult = new ErrorResult();
        
        // change status to "Draft" if status is "Ready"
        if (Equals(VeaEventStatus, VeaEventStatus.Ready))
        {
            VeaEventStatus = VeaEventStatus.Draft;
        }
        
        // cause error if status is "Active" or "Cancelled"
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Title of active event cannot be changed")));
        }
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Title of cancelled event cannot be changed")));
        }
        

        if (errorResult.HasErrors())
        {
            return errorResult;
        }
        
        Title = title;
        return new Result();
    }
    
}