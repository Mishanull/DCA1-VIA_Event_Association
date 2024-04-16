using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class SingleEventPageQueryHandler(VeaDbContext context) : IQueryHandler<SingleEventPage.Query,SingleEventPage.Answer>
{
    public async Task<SingleEventPage.Answer> HandleAsync(SingleEventPage.Query query)
    {
        var singleEvent = await context.Events
            .Where(e => e.Id == query.EventId)
            .Select(e => new SingleEventPage.Event(e.Title, e.Description, e.Location.LocationName, e.From, e.To, e.VeaEventType, e.MaxGuests))
            .FirstOrDefaultAsync();
        
        //event's invites
        var inviteGuestIds = await context.Invites
            .Where(i => i.EventId == query.EventId && i.InviteStatus == "Accepted")
            .Select(i => i.GuestId)
            .ToListAsync();
        //invited Guests
        var invitedGuests = context.Guests
            .Where(g => inviteGuestIds.Contains(g.Id));
        
        //event's requests
        var requestGuestIds = await context.Requests
            .Where(r => r.EventId == query.EventId && r.RequestStatus == "Approved")
            .Select(r => r.GuestId)
            .ToListAsync();
        //requested Guests
        var requestedGuests = context.Guests
            .Where(g => requestGuestIds.Contains(g.Id));
        
        //merge guests
        var guests = await requestedGuests.Concat(invitedGuests)
            .Skip(query.RowSize * (query.PageNumber-1))
            .Take(query.DisplayedRows * query.RowSize)
            .Select(g => new SingleEventPage.Guest(
                g.Id,
                g.FirstName,
                g.PictureUrl)
            ).ToListAsync();

        return new SingleEventPage.Answer(singleEvent, guests, guests.Count);
    }
}