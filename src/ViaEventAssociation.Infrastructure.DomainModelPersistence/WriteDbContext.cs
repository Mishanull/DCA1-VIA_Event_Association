using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EntityM_Trial;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence;

public class WriteDbContext(DbContextOptions options):DbContext(options)
{
    // public DbSet<Creator> Creators => Set<Creator>();
    // public DbSet<VeaEvent> Events => Set<VeaEvent>();
    // public DbSet<VeaGuest> Guests => Set<VeaGuest>();
    // public DbSet<Location> Locations => Set<Location>();
    public DbSet<EntityM> EntityMs => Set<EntityM>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        
        //apply configurations from configuration classes
        // modelBuilder.ApplyConfiguration(new LocationConfig());
    }
}