namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Creator
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
