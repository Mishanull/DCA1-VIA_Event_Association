using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEvent (VeaEventId id) : AggregateRoot(id)
{
    // Default values
    private const string DefaultTitle = "Working Title";
    private const string DefaultDescription = "";
    private static readonly VeaEventStatus DefaultEventStatus = VeaEventStatus.Draft;
    private const int DefaultMaxGuests = 5;
    private static readonly VeaEventType DefaultEventType = VeaEventType.Private;
    
    // Services
    internal ICurrentTime currentTimeProvider;
    
    // Properties
    internal Title Title { get; set; } = ((Result<Title>)Title.Create(DefaultTitle)).Value;
    internal Description Description { get; set; } = ((Result<Description>)Description.Create(DefaultDescription)).Value;
    internal VeaEventType VeaEventType { get; set; } = DefaultEventType;
    internal MaxGuests MaxGuests { get; set; } = ((Result<MaxGuests>)MaxGuests.Create(DefaultMaxGuests)).Value;
    internal VeaEventStatus VeaEventStatus { get; set; } = DefaultEventStatus;
    internal FromTo FromTo { get; set; }
    // internal CreatorId CreatorId { get; }
    // internal LocationId LocationId { get; }
    // internal List<GuestId> Participants { get; }
    
    // Constructors
    private VeaEvent(VeaEventId id, ICurrentTime currentTimeProvider) : this(id)
    {
        this.currentTimeProvider = currentTimeProvider;
    }
    
    public static Result<VeaEvent> Create(ICurrentTime currentTime)
    {
        var veaEvent = new VeaEvent(new VeaEventId(), currentTime);
        return new Result<VeaEvent>(veaEvent);
    }

    internal Result UpdateTitle(Title title)
    {
        // change status to "Draft" if status is "Ready"
        if (Equals(VeaEventStatus, VeaEventStatus.Ready))
        {
            VeaEventStatus = VeaEventStatus.Draft;
        }
        
        var errorResult = new Result();
        // cause error if status is "Active" or "Cancelled"
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Title of active event cannot be changed")));
        }
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Title of cancelled event cannot be changed")));
        }
        
        if (errorResult.IsErrorResult()) { return errorResult; }
        Title = title;
        return new Result();
    }

    public Result UpdateDescription(Description description)
    {
        // change status to "Draft" if status is "Ready"
        if (Equals(VeaEventStatus, VeaEventStatus.Ready))
        {
            VeaEventStatus = VeaEventStatus.Draft;
        }
        
        var errorResult = new Result();
        // cause error if status is "Cancelled"
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Description of cancelled event cannot be changed")));
        }
        // cause error if status is "Active"
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("Description of active event cannot be changed")));
        }
        
        if (errorResult.IsErrorResult()) { return errorResult; }
        Description = description;
        return new Result();
    }

    public Result UpdateFromTo(FromTo fromTo)
    {
        //change status to "Draft" if status is "Ready"
        if (Equals(VeaEventStatus, VeaEventStatus.Ready))
        {
            VeaEventStatus = VeaEventStatus.Draft;
        }
        
        var errorResult = new Result();
        // cannot update fromTo of active event
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("FromTo of active event cannot be changed")));
        }
        // cannot update fromTo of cancelled event
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed, new ErrorMessage("FromTo of cancelled event cannot be changed")));
        }
        // event cannot be shorter than 1 hour and longer than 10 hours
        if (fromTo.End - fromTo.Start < TimeSpan.FromHours(1) || fromTo.End - fromTo.Start > TimeSpan.FromHours(10))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Event cannot be shorter than 1 hour and longer than 10 hours")));
        }
        // event cannot start in the past
        // if (fromTo.Start < DateTime.Now)
        if (fromTo.Start < currentTimeProvider.GetCurrentTime())
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Event cannot start in the past")));
        }
        // event cannot start after midnight or it cannot start before 8am
        if (TimeOnly.FromDateTime(fromTo.Start) >= new TimeOnly(0, 0, 0) && TimeOnly.FromDateTime(fromTo.Start) < new TimeOnly(8, 0, 0))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Event cannot start after midnight or it cannot start before 8am")));
        }
        // event cannot end after 1am the next day
        if (DateOnly.FromDateTime(fromTo.End) > DateOnly.FromDateTime(fromTo.Start) && TimeOnly.FromDateTime(fromTo.End) > new TimeOnly(1, 0, 0))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Event cannot end after 1am")));
        }
        
        if (errorResult.IsErrorResult()) { return errorResult; }
        FromTo = fromTo;
        return new Result();
    }
}