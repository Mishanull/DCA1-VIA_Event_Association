using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class ProfilePageQueryHandler(VeaDbContext context) : IQueryHandler<ProfilePage.Query,ProfilePage.Answer>
{
    public async Task<ProfilePage.Answer> HandleAsync(ProfilePage.Query query)
    {
        var guest = await context.Guests
            .Where(g => g.Id == query.GuestId)
            .Select(g => new ProfilePage.Guest(g.Id, g.FirstName, g.LastName, g.Email, g.PictureUrl))
            .FirstOrDefaultAsync();
        
        //guest's approved requests
        var requestEventIds = await context.Invites
            .Where(i => i.GuestId == query.GuestId && i.InviteStatus == "Accepted")
            .Select(i => i.EventId)
            .ToListAsync();
        //events of these requests
        var requestEvents = context.Events
            .Where(e => requestEventIds.Contains(e.Id));
        
        //guest's accepted invites
        var inviteEventIds = await context.Invites
            .Where(i => i.GuestId == query.GuestId && i.InviteStatus == "Accepted")
            .Select(i => i.EventId)
            .ToListAsync();
        //events of these invites
        var inviteEvents = context.Events
            .Where(e => inviteEventIds.Contains(e.Id));
        
        //merge the two queriables into one
        var events = requestEvents.Concat(inviteEvents);
        
        //get the total number of invitations for each of these events
        var numberOfInvitations = await context.Invites
            .Where(i => events.Select(e => e.Id).Contains(i.EventId))
            .GroupBy(i => i.EventId)
            .Select(g => new {EventId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.EventId, g => g.Count);
        
        //get the total number of requests for each of these events
        var numberOfRequests = await context.Requests
            .Where(r => events.Select(e => e.Id).Contains(r.EventId) && r.RequestStatus == "Approved")
            .GroupBy(r => r.EventId)
            .Select(g => new {EventId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.EventId, g => g.Count);
        
        //combine the events with the number of invitations and requests
        var eventsWithParticipants = await events
            .Select(e => new ProfilePage.Event(
                e.Id,
                e.Title,
                numberOfInvitations.GetValueOrDefault(e.Id)+numberOfRequests.GetValueOrDefault(e.Id),
                e.From)
            ).ToListAsync();
        
        var guestPendingInvitesCount = context.Invites
            .Count(i => i.GuestId == query.GuestId && i.InviteStatus == "Pending");
        
        return new ProfilePage.Answer(guest!, eventsWithParticipants, guestPendingInvitesCount);
    }
}