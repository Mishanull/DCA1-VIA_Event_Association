using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.CreatorAgg;

public class Creator : AggregateRoot
{
    internal CreatorId Id { get; }
    internal Email Email { get; private init; }

    internal ICollection<Invite> CreatedInvites { get; private init; } = [];

    private Creator(CreatorId id) : base(id)
    {
        Id = id;
    }
    public static Result<Creator> Create(Email email)
    {
        email.Value = email.Value.ToLower();
        var creator = new Creator(new CreatorId())
        {
            Email = email,
        };

        return new Result<Creator>(creator);
    }

    public void AddInvite(Invite invite)
    {
        CreatedInvites.Add(invite);
    }
}