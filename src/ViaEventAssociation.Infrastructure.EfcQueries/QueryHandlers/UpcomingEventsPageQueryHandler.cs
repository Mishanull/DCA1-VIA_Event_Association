using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class UpcomingEventsPageQueryHandler(VeaDbContext context, ICurrentTime currentTime) : IQueryHandler<UpcomingEventsPage.Query, UpcomingEventsPage.Answer>
{
    public async Task<UpcomingEventsPage.Answer> HandleAsync(UpcomingEventsPage.Query query)
    {
        var time = currentTime.GetCurrentTime().ToString("yyyy-MM-ddTHH:mm:ss");
        var allUpcomingEvents = context.Events
            .Where(e => 
                e.From.CompareTo(time) > 0 
                && e.Title.Contains(query.SearchedText)
            );

        var upcomingEvents = allUpcomingEvents
            .OrderByDescending(e => e.From)
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize);
        
        //get the total number of invitations for each of these events
        var numberOfInvitations = await context.Invites
            .Where(i => upcomingEvents.Select(e => e.Id).Contains(i.EventId))
            .GroupBy(i => i.EventId)
            .Select(g => new {EventId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.EventId, g => g.Count);
        
        //get the total number of requests for each of these events
        var numberOfRequests = await context.Requests
            .Where(r => upcomingEvents.Select(e => e.Id).Contains(r.EventId))
            .GroupBy(r => r.EventId)
            .Select(g => new {EventId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.EventId, g => g.Count);
        
        //combine the events with the number of invitations and requests
        var upcomingEventsWithParticipants = await upcomingEvents
            .Select(e => new UpcomingEventsPage.Event(
                e.Id,
                e.From,
                e.Title,
                e.Description,
                e.MaxGuests,
                e.VeaEventType,
                numberOfInvitations.GetValueOrDefault(e.Id) + numberOfRequests.GetValueOrDefault(e.Id))
            ).ToListAsync();

        //calculate max page number
        var maxPageNum = allUpcomingEvents.Count() / query.PageSize;
        if (maxPageNum % query.PageSize > 0)
        {
            maxPageNum++;
        }
        
        return new UpcomingEventsPage.Answer(upcomingEventsWithParticipants, maxPageNum);
    }
}