namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Guest
{
    public string Id { get; set; } = null!;

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PictureUrl { get; set; }

    public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
