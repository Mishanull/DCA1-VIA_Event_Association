using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.LocationAgg;
using ViaEventAssociation.Infrastructure.SqliteDmPersistence.EntityM_Trial;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence;

public class WriteDbContext(DbContextOptions options):DbContext(options)
{
    public DbSet<Creator> Creators => Set<Creator>();
    public DbSet<VeaEvent> Events => Set<VeaEvent>();
    public DbSet<VeaGuest> Guests => Set<VeaGuest>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<EntityM> EntityMs => Set<EntityM>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        
        ConfigureRequestEntity(modelBuilder.Entity<Request>());
        ConfigureInviteEntity(modelBuilder.Entity<Invite>());
    }
    
    //Configure Event-Guest many-to-many relationship using Request as the join entity/table
    private void ConfigureRequestEntity(EntityTypeBuilder<Request> propertyTypeBuilder)
    {
        //Id : strongly-typed PK
        propertyTypeBuilder.HasKey(r=>r.Id);
        propertyTypeBuilder
            .Property(r => r.Id)
            .HasConversion(
                rId=>rId.Value,
                dbValue=>TId.FromGuid<RequestId>(dbValue).Value!
            );
            
        //Reason : primitive property
        propertyTypeBuilder
            .Property<string>(r => r.Reason);
        
        //EventId : strongly-typed FK
        propertyTypeBuilder
            .Property(r=>r.EventId)
            .HasConversion(
                eId => eId.Value,
                dbValue => TId.FromGuid<VeaEventId>(dbValue).Value!
            );
            
        //GuestId : strongly-typed FK
        propertyTypeBuilder
            .Property(r=>r.GuestId)
            .HasConversion(
                gId => gId.Value,
                dbValue => TId.FromGuid<GuestId>(dbValue).Value!
            );
        
        //RequestStatus : enum
        propertyTypeBuilder
            .Property<RequestStatus>(r => r.RequestStatus)
            .HasConversion(
                status => status.DisplayName,
                value => (RequestStatus)Enum.Parse(typeof(RequestStatus), value)
            );
        
        //constraint : combination of EventId and GuestId is unique
        propertyTypeBuilder
            .HasIndex(r => new {r.EventId, r.GuestId})
            .IsUnique();
        
        //Many-to-many relationship
        propertyTypeBuilder
            .HasOne<VeaEvent>()
            .WithMany()
            .HasForeignKey(r => r.EventId);
        propertyTypeBuilder
            .HasOne<VeaGuest>()
            .WithMany()
            .HasForeignKey(r => r.GuestId);
    }
    
    private void ConfigureInviteEntity(EntityTypeBuilder<Invite> propertyTypeBuilder)
    {
        //Id : strongly-typed PK
        propertyTypeBuilder.HasKey(i=>i.Id);
        propertyTypeBuilder.Property(i => i.Id)
            .HasConversion(
                iId => iId.Value,
                dbValue => TId.FromGuid<InviteId>(dbValue).Value!
            );
        
        //InviteStatus : enum
        propertyTypeBuilder.Property<InviteStatus>(i => i.InviteStatus)
            .HasConversion(
                status => status.DisplayName,
                value => (InviteStatus)Enum.Parse(typeof(InviteStatus), value)
            );
        
        //Timestamp : primitive property
        propertyTypeBuilder.Property(i => i.Timestamp);
        
        //CreatorId : strongly-typed FK
        propertyTypeBuilder.Property(i => i.CreatorId)
            .HasConversion(
                cId => cId.Value,
                dbValue => TId.FromGuid<CreatorId>(dbValue).Value!
            );
        
        //GuestId : strongly-typed FK
        propertyTypeBuilder.Property(i => i.GuestId)
            .HasConversion(
                gId => gId.Value,
                dbValue => TId.FromGuid<GuestId>(dbValue).Value!
            );
        
        //EventId : strongly-typed FK
        propertyTypeBuilder.Property(i => i.EventId)
            .HasConversion(
                eId => eId.Value,
                dbValue => TId.FromGuid<VeaEventId>(dbValue).Value!
            );
        
        //relationships
        propertyTypeBuilder
            .HasOne<Creator>()
            .WithMany(c => c.CreatedInvites)
            .HasForeignKey(i => i.CreatorId);
        
        propertyTypeBuilder.HasOne<VeaGuest>()
            .WithMany()
            .HasForeignKey(i => i.GuestId);
        
        propertyTypeBuilder
            .HasOne<VeaEvent>()
            .WithMany()
            .HasForeignKey(i => i.EventId);
    } 
}