namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Request
{
    public string Id { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string EventId { get; set; } = null!;

    public string GuestId { get; set; } = null!;

    public string RequestStatus { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
