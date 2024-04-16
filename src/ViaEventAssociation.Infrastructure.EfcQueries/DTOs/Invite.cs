namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Invite
{
    public string Id { get; set; } = null!;

    public string InviteStatus { get; set; } = null!;

    public string Timestamp { get; set; } = null!;

    public string CreatorId { get; set; } = null!;

    public string GuestId { get; set; } = null!;

    public string EventId { get; set; } = null!;

    public virtual Creator Creator { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
