using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeInviteRepository : IInviteRepository
{

    public Result<Invite> Find(InviteId id)
    {
        var guestId = Guid.NewGuid().ToString();
        var creatorId = Guid.NewGuid().ToString();
        var eventId = Guid.NewGuid().ToString();
        return new Result<Invite>(Invite.Create(new GuestId(),
            new CreatorId(), new VeaEventId()).Value);
    }
    
    public async Task<Result<Invite>> FindAsync(InviteId id)
    {
        var guestId = Guid.NewGuid().ToString();
        var creatorId = Guid.NewGuid().ToString();
        var eventId = Guid.NewGuid().ToString();
        return new Result<Invite>(Invite.Create(new GuestId(),
            new CreatorId(), new VeaEventId()).Value);
    }
    
    public async Task<Result> AddAsync(Invite entity)
    {
        return new Result();

    }
    
    public Result Remove(InviteId id)
    {
        throw new NotImplementedException();
    }
    
    public Task<Result> RemoveAsync(InviteId id)
    {
        throw new NotImplementedException();
    }
    
    public Result<Invite> Update(Invite entity)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<Invite>> UpdateAsync(Invite entity)
    {
        return new Result<Invite>(entity);
    }
}