using ViaEventAssociation.Core.QueryContracts.Contract;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class SingleEventPage
{
    public record Query(string EventId, int PageNumber, int DisplayedRows, int RowSize) : IQuery<Answer>;
    public record Answer(Event Event, List<Guest> Guest, int GuestsCount);

    public record Event(
        string Title,
        string Description,
        string LocationName,
        string From,
        string To,
        string Type,
        int MaxGuests
    );
    
    public record Guest(
        string Id,
        string FirstName,
        string PictureUrl
    );
}