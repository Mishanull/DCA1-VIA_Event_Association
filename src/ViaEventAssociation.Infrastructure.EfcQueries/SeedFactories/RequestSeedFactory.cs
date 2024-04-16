using System.Text.Json;

namespace ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

public record TmpRequest(string EventId, string GuestId, string Reason, string Status);
public class RequestSeedFactory
{
    public static List<DTOs.Request> CreateRequests()
    {
        //const:
        var tmpRequests = JsonSerializer.Deserialize<List<TmpRequest>>(RequestsJson);

        var requests = tmpRequests!.Select(r=> new DTOs.Request()
        {
            Id = Guid.NewGuid().ToString(),
            Reason = r.Reason,
            EventId = r.EventId,
            GuestId = r.GuestId,
            RequestStatus = r.Status
        }).ToList();

        return requests;
    }
    
    private const string RequestsJson = """
                                       [
                                           {
                                             "EventId": "27a5bde5-3900-4c45-9358-3d186ad6b2d7",
                                             "GuestId": "9a61ec3a-348d-473d-acb4-3d9decb0eb55",
                                             "Reason": "I want to meet new people",
                                             "Status": "Approved"
                                           },
                                           {
                                             "EventId": "27a5bde5-3900-4c45-9358-3d186ad6b2d7",
                                             "GuestId": "d6ef00cc-7862-4660-a78d-e966273cacb6",
                                             "Reason": "I\u0027m up for a challenge",
                                             "Status": "Pending"
                                           },
                                           {
                                             "EventId": "27a5bde5-3900-4c45-9358-3d186ad6b2d7",
                                             "GuestId": "4bf6523a-0667-41c4-acc9-d09a4e9af430",
                                             "Reason": "This sounds interesting",
                                             "Status": "Pending"
                                            }
                                       ]
                                       """;
}