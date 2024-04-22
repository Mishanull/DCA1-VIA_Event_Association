using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contract;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class AvailableLocationsPageQueryHandler(VeaDbContext context)
    : IQueryHandler<AvailableLocationsPage.Query, AvailableLocationsPage.Answer>
{
    public async Task<AvailableLocationsPage.Answer> HandleAsync(AvailableLocationsPage.Query query)
    {
        var veaEvent = await context.Events.Where(e => e.Id == query.EventId)
            .Select(e => new AvailableLocationsPage.Event(e.Title!, e.CreatorId, e.From!, e.To!)).FirstOrDefaultAsync();

        var creatorEmail = await context.Creators.Where(c => c.Id == veaEvent!.CreatorId).Select(c => c.Email)
            .FirstOrDefaultAsync();
        
        //this is needed to specify which format to parse the date with
        string locationDateFormat = "dd-MM-yyyy";
        string eventDateFormat = "yyyy-MM-dd HH:mm";
        CultureInfo provider = CultureInfo.InvariantCulture;
        var locationList = await context.Locations
            .Select(l => new AvailableLocationsPage.Location(l.Id, l.LocationName, l.Start, l.End)).ToListAsync();
        locationList = locationList.Where(l =>
            DateTime.ParseExact(l.From, locationDateFormat, provider).Date <= DateTime.ParseExact(veaEvent!.From, eventDateFormat, provider).Date &&
            DateTime.ParseExact(l.To, locationDateFormat, provider).Date >= DateTime.ParseExact(veaEvent.To, eventDateFormat, provider).Date).ToList();
        
        return new AvailableLocationsPage.Answer(locationList, veaEvent!, creatorEmail!);
    }
}