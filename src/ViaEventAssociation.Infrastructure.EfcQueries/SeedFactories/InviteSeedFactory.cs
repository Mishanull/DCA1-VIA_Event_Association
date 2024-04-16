using System.Text.Json;

namespace ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

public record TmpInvite(string EventId, string GuestId, string Status);
public class InviteSeedFactory
{
  
  public static List<DTOs.Invite> CreateInvites(string creatorId)
  {
    //const:
    var tmpInvites = JsonSerializer.Deserialize<List<TmpInvite>>(InvitesJson);

    var invites = tmpInvites!.Select(i=> new DTOs.Invite()
    {
      Id = Guid.NewGuid().ToString(),
      InviteStatus = i.Status,
      CreatorId = creatorId,
      GuestId = i.GuestId,
      EventId = i.EventId,
      Timestamp = DateTime.Now.ToString()
    }).ToList();

    return invites;
  }
  
  private const string InvitesJson = """
                                     [
                                       {
                                       "EventId": "27a5bde5-3900-4c45-9358-3d186ad6b2d7",
                                       "GuestId": "6d0e5ef9-294e-4cfb-9575-54427640b6a4",
                                       "Status": "Accepted"
                                       }
                                     ]
                                     """;
}