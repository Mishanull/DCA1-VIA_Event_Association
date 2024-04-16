using ViaEventAssociation.Core.QueryContracts.Contract;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public abstract class ProfilePage
{
    public record Query(string GuestId, int PageNumber, int PageSize) : IQuery<Answer>;
    public record Answer(Guest Guest ,List<Event> Events, int GuestPendingInvitesCount);

    public record Event(
        string Id,
        string Title,
        int GuestsCount,
        string From
    );
    public record Guest(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string PictureUrl
    );
}