using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeGuestRepository : IGuestRepository
{
    public async Task<Result<VeaGuest>> FindAsync(GuestId id)
    {
        return new Result<VeaGuest>(VeaGuest.Create(
            Email.Create("example@via.dk", new FakeEmailCheck()).Value!,
            FirstName.Create("marius").Value!,
            LastName.Create("posda").Value!,
            PictureUrl.Create("https://example.com/yes.png").Value!).Value);
    }

    public async Task<Result> AddAsync(VeaGuest entity)
    {
        return new Result();
    }

    public Task<Result> RemoveAsync(GuestId id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Request>> FindRequestAsync(RequestId id)
    {
        return new Result<Request>(Request.Create("I am interested in this.", new VeaEventId(), new GuestId()).Value!);
    }
}