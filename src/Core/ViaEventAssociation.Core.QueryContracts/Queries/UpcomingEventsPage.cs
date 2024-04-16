using ViaEventAssociation.Core.QueryContracts.Contract;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class UpcomingEventsPage
{
    public record Query(int PageNumber, int PageSize, string SearchedText) : IQuery<Answer>;
    public record Answer(List<Event> Events, int MaxPageNum);
    
    public record Event(
        string Id, 
        string From,
        string Title,
        string Description,
        int MaxGuests,
        string Type,
        int GuestsCount
    );
}