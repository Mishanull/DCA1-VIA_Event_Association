using ViaEventAssociation.Core.QueryContracts.Contract;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class UnpublishedEventsPage
{
    public record Query(string CreatorId) : IQuery<Answer>;

    public record Answer(
        List<Event> DraftEvents,
        List<Event> ReadyEvents,
        List<Event> CancelledEvents,
        List<Event> ActiveEvents
    );
        
    public record Event(
        string Id,
        string Title,
        string Status
    );
}