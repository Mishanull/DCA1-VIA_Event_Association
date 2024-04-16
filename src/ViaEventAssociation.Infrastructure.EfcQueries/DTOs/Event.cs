namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Event
{
    public string Id { get; set; } = null!;

    public string VeaEventType { get; set; } = null!;

    public string VeaEventStatus { get; set; } = null!;

    public string? From { get; set; }

    public string? To { get; set; }

    public string CreatorId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string Description { get; set; } = null!;

    public int MaxGuests { get; set; }

    public string? Title { get; set; }

    public virtual Creator Creator { get; set; } = null!;

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
