using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class UnpublishedEventsPageQueryHandler(VeaDbContext context) : IQueryHandler<UnpublishedEventsPage.Query,UnpublishedEventsPage.Answer>
{
    public async Task<UnpublishedEventsPage.Answer> HandleAsync(UnpublishedEventsPage.Query query)
    {
        //events of the creator
        var creatorEvents = context.Events
            .Where(e => e.CreatorId == query.CreatorId);
            
        //Draft events
        var draftEvents = await creatorEvents
            .Where(ce => ce.VeaEventStatus == "draft")
            .Select(e => new UnpublishedEventsPage.Event(e.Id, e.Title, e.VeaEventStatus))
            .ToListAsync();
        
        //Ready events
        var readyEvents = await creatorEvents
            .Where(ce => ce.VeaEventStatus == "ready")
            .Select(e => new UnpublishedEventsPage.Event(e.Id, e.Title, e.VeaEventStatus))
            .ToListAsync();

        //Cancelled events
        var cancelledEvents = await creatorEvents
            .Where(ce => ce.VeaEventStatus == "cancelled")
            .Select(e => new UnpublishedEventsPage.Event(e.Id, e.Title, e.VeaEventStatus))
            .ToListAsync();
            
        //Active events
        var activeEvents = await creatorEvents
            .Where(ce => ce.VeaEventStatus == "active")
            .Select(e => new UnpublishedEventsPage.Event(e.Id, e.Title, e.VeaEventStatus))
            .ToListAsync();
        
        return new UnpublishedEventsPage.Answer(draftEvents, readyEvents, cancelledEvents, activeEvents);
    }
}