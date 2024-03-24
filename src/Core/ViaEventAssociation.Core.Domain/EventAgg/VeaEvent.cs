using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.EventAgg;

public class VeaEvent(VeaEventId id) : AggregateRoot(id)
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
    internal VeaEventId Id { get; } = id;
    internal Title Title { get; set; } = ((Result<Title>)Title.Create(DefaultTitle)).Value;
    internal Description Description { get; set; } = ((Result<Description>)Description.Create(DefaultDescription)).Value;
    internal VeaEventType VeaEventType { get; set; } = DefaultEventType;
    internal MaxGuests MaxGuests { get; set; } = ((Result<MaxGuests>)MaxGuests.Create(DefaultMaxGuests)).Value;
    internal VeaEventStatus VeaEventStatus { get; set; } = DefaultEventStatus;
    internal FromTo FromTo { get; set; }
    internal CreatorId CreatorId { get; }
    // internal LocationId LocationId { get; }
    internal List<GuestId> Participants { get; } = [];

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

    // Methods
    public Result UpdateTitle(Title title)
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
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed,
                new ErrorMessage("Title of active event cannot be changed")));
        }

        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed,
                new ErrorMessage("Title of cancelled event cannot be changed")));
        }

        if (errorResult.IsErrorResult())
        {
            return errorResult;
        }

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
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed,
                new ErrorMessage("Description of cancelled event cannot be changed")));
        }

        // cause error if status is "Active"
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(new VeaError(ErrorType.ActionNotAllowed,
                new ErrorMessage("Description of active event cannot be changed")));
        }

        if (errorResult.IsErrorResult())
        {
            return errorResult;
        }

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
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            errorResult.CollectError(ErrorHelper.CreateVeaError("Cancelled event cannot be modified.", ErrorType.ActionNotAllowed));
        }
        
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            errorResult.CollectError(ErrorHelper.CreateVeaError("Active event cannot be modified.", ErrorType.ActionNotAllowed));
        }
        
        // event cannot be shorter than 1 hour and longer than 10 hours
        if (fromTo.End - fromTo.Start < TimeSpan.FromHours(1) || fromTo.End - fromTo.Start > TimeSpan.FromHours(10))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo,
                new ErrorMessage("Event cannot be shorter than 1 hour and longer than 10 hours")));
        }

        // event cannot start in the past
        // if (fromTo.Start < DateTime.Now)
        if (fromTo.Start < currentTimeProvider.GetCurrentTime())
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo,
                new ErrorMessage("Event cannot start in the past")));
        }

        // event cannot start after midnight or it cannot start before 8am
        if (TimeOnly.FromDateTime(fromTo.Start) >= new TimeOnly(0, 0, 0) &&
            TimeOnly.FromDateTime(fromTo.Start) < new TimeOnly(8, 0, 0))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo,
                new ErrorMessage("Event cannot start after midnight or it cannot start before 8am")));
        }

        // event cannot end after 1am the next day
        if (DateOnly.FromDateTime(fromTo.End) > DateOnly.FromDateTime(fromTo.Start) &&
            TimeOnly.FromDateTime(fromTo.End) > new TimeOnly(1, 0, 0))
        {
            errorResult.CollectError(new VeaError(ErrorType.InvalidFromTo,
                new ErrorMessage("Event cannot end after 1am")));
        }

        if (errorResult.IsErrorResult())
        {
            return errorResult;
        }

        FromTo = fromTo;
        return new Result();
    }

    public Result MakePublic()
    {
        var result = new Result();
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            result.CollectError(new VeaError(ErrorType.InvalidStatus,
                new ErrorMessage("Cancelled event cannot be set to 'public'")));
            return result;
        }

        VeaEventType = VeaEventType.Public;
        return result;
    }

    public Result MakePrivate()
    {
        var result = new Result();

        if (Equals(VeaEventStatus.Active, VeaEventStatus) || Equals(VeaEventStatus.Cancelled, VeaEventStatus))
        {
            result.CollectError(new VeaError(ErrorType.InvalidStatus,
                new ErrorMessage($"{VeaEventStatus.DisplayName} event cannot be set to 'private'")));
            return result;
        }

        if (Equals(VeaEventType.Private, VeaEventType))
        {
            return result;
        }

        VeaEventType = VeaEventType.Private;
        VeaEventStatus = VeaEventStatus.Draft;
        return result;
    }

    public Result SetMaxGuests(MaxGuests maxGuests)
    {
        var result = new Result();
        if (Equals(VeaEventStatus.Cancelled, VeaEventStatus))
        {
            result.CollectError(new VeaError(ErrorType.InvalidMaxGuests,
                new ErrorMessage("Cancelled event cannot be modified.")));
            return result;
        }

        if (Equals(VeaEventStatus.Draft, VeaEventStatus) || Equals(VeaEventStatus.Ready, VeaEventStatus))
        {
            MaxGuests = maxGuests;
        }

        if (Equals(VeaEventStatus.Active, VeaEventStatus))
        {
            if (maxGuests.Value < MaxGuests.Value)
            {
                result.CollectError(new VeaError(ErrorType.InvalidMaxGuests,
                    new ErrorMessage("Maximum number of guests of an active event cannot be reduced.")));
                return result;
            }

            MaxGuests = maxGuests;
        }

        return result;
    }

    // Naming? 
    public Result Ready()
    {
        var result = new Result();
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            result.CollectError(new VeaError(ErrorType.InvalidStatus, new ErrorMessage("Cancelled event cannot be readied.")));
            return result;
        }

        if (FromTo is null)
        {
            result.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Time not set")));
        }
        else if (FromTo.Start < currentTimeProvider.GetCurrentTime())
        {
            result.CollectError(new VeaError(ErrorType.InvalidFromTo, new ErrorMessage("Event starting in the past cannot be readied.")));
            return result;
        }

        if (Equals(Title.Value, DefaultTitle) || Equals(true, string.IsNullOrEmpty(Title.Value)))
        {
            result.CollectError(new VeaError(ErrorType.InvalidTitle, new ErrorMessage("Title must be set and changed from the default value")));
        }

        if (Equals(Description.Value, DefaultDescription) || Equals(true, string.IsNullOrEmpty(Description.Value)))
        {
            result.CollectError(new VeaError(ErrorType.InvalidDescription, new ErrorMessage("Description not set")));
        }

        if (!(Equals(VeaEventType, VeaEventType.Private) || Equals(VeaEventType, VeaEventType.Public)))
        {
            result.CollectError(new VeaError(ErrorType.ValidationFailed, new ErrorMessage("Event visibility not set")));
        }

        if (MaxGuests.Value is < 5 or > 50)
        {
            result.CollectError(new VeaError(ErrorType.InvalidMaxGuests, new ErrorMessage("Max number of guests must be at least 5 and at most 50")));
        }

        if (result.IsErrorResult())
        {
            return result;
        }

        VeaEventStatus = VeaEventStatus.Ready;
        return result;
    }

    public Result Activate()
    {
        var result = new Result();
        // if cancelled, return error
        if (Equals(VeaEventStatus, VeaEventStatus.Cancelled))
        {
            result.CollectError(new VeaError(ErrorType.InvalidStatus, new ErrorMessage("Cancelled event cannot be activated")));
            return result;
        }
        // if ready, make active
        if (Equals(VeaEventStatus, VeaEventStatus.Ready))
        {
            VeaEventStatus = VeaEventStatus.Active;
            return result;
        }
        // if active, don't change anything
        if (Equals(VeaEventStatus, VeaEventStatus.Active))
        {
            return result;
        }
        // if in draft, ready first
        result = Ready();
        if (result.IsErrorResult())
        {
            return result;
        }

        VeaEventStatus = VeaEventStatus.Active;
        return result;

        return result;
    }

    public void AddParticipant(GuestId guestId)
    {
        Participants.Add(guestId);
    }

    public void RemoveParticipant(GuestId guestId)
    {
        Participants.Remove(guestId);
    }

    public int ParticipantCount()
    {
        return Participants.Count();
    }

    public bool IsFull()
    {
        return Participants.Count == MaxGuests.Value;
    }

    public bool IsParticipant(GuestId guestId)
    {
        return Participants.Contains(guestId);
    }
}