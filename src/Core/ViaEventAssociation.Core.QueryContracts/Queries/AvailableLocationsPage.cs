using ViaEventAssociation.Core.QueryContracts.Contract;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public abstract class AvailableLocationsPage
{
    public record Query(string EventId):  IQuery<Answer>;

    public record Answer(List<Location> Locations, Event Event, string CreatorEmail);

    public record Event(
        string Title,
        string CreatorId,
        string From,
        string To
    );

    public record Location(
        string Id,
        string Name,
        string From,
        string To
    );
}