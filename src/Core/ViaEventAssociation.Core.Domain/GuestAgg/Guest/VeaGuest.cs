using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.GuestAgg.Guest;

public  class GuestId : TId
{
    public GuestId()
    {
        
    }
    
    internal GuestId(string id) : base(id)
    {
    }
}

public class VeaGuest : AggregateRoot
{
    internal GuestId Id { get; }
    internal Email? Email { get; private init; } 
    internal FirstName? FirstName{ get; private init; } 
    internal LastName? LastName { get; private init; } 
    internal PictureUrl? PictureUrl { get; private init; }
    internal ICollection<RequestEntity.Request> JoinRequests { get; private init; } = [];

    internal VeaGuest(GuestId id) : base(id)
    {
        Id = id;
    }
    
    public static Result<VeaGuest> Create(Email email, FirstName firstName, LastName lastName, PictureUrl pictureUrl)
    {
        
        email.Value = email.Value.ToLower();
        if (!string.IsNullOrEmpty(firstName.Value))
        {
            firstName.Value = char.ToUpper(firstName.Value[0]) + firstName.Value.Substring(1).ToLower();
        }

        if (!string.IsNullOrEmpty(lastName.Value))
        {
            lastName.Value = char.ToUpper(lastName.Value[0]) + lastName.Value.Substring(1).ToLower();
        }

        var guest = new VeaGuest(new GuestId())
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PictureUrl = pictureUrl
        };
        
        return new Result<VeaGuest>(guest);
    }

    public void AddRequest(RequestEntity.Request request)
    {
        JoinRequests.Add(request);
    }
}