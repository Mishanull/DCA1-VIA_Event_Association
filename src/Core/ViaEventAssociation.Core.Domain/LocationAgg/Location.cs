using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Domain.LocationAgg;

public class Location
{
   internal LocationId Id { get; private init; } = new LocationId();
   internal LocationName Name { get; private set; }
   internal FromTo FromTo { get; private set; } = FromTo.Create(DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(7d))).Value!;
   internal MaxGuests MaxGuests { get; private set; } = MaxGuests.Create(5).Value!;
   internal CreatorId CreatorId { get; private init; }
   private Location( CreatorId creatorId, LocationName name) 
   {
      Name = name;
      CreatorId = creatorId;
   }

   public static Result<Location> Create(LocationName name, CreatorId creatorId )
   {
      return new Result<Location>(new Location(creatorId, name));
   }

   public void UpdateName(LocationName locationName)
   {
      Name = locationName;
   }

   public void SetMaxGuests(MaxGuests maxGuests)
   {
      MaxGuests = maxGuests;
   }
   
   public void SetFromTo(FromTo fromTo)
   {
      FromTo = fromTo;
   }
}