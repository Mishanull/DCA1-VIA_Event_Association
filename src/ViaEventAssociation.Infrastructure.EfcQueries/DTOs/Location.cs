namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Location
{
    public string Id { get; set; } = null!;

    public string CreatorId { get; set; } = null!;

    public string End { get; set; } = null!;

    public string Start { get; set; } = null!;

    public int MaxGuests { get; set; }

    public string LocationName { get; set; } = null!;

    public virtual Creator Creator { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
