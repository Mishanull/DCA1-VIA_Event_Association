using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.FakeServices.Repositories;

public class FakeGuestRepository : IGuestRepository
{

    public Result<VeaGuest> Find(GuestId id)
    {
         return new Result<VeaGuest>(VeaGuest.Create(
            Email.Create("example@via.dk", new FakeEmailCheck()).Value!, 
            FirstName.Create("marius").Value!, 
            LastName.Create("posda").Value!, 
            PictureUrl.Create("https://example.com/yes.png").Value!).Value);
    }
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

    public Result Remove(GuestId id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveAsync(GuestId id)
    {
        throw new NotImplementedException();
    }
    public Result<VeaGuest> Update(VeaGuest entity)
    {
        throw new NotImplementedException();
    }
    public async Task<Result<VeaGuest>> UpdateAsync(VeaGuest entity)
    {
        return new Result<VeaGuest>(entity);
    }
}