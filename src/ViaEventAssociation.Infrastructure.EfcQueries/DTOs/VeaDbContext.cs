using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.SeedFactories;

namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class VeaDbContext : DbContext
{
    public VeaDbContext()
    {
    }

    public VeaDbContext(DbContextOptions<VeaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Creator> Creators { get; set; }

    public virtual DbSet<EntityM> EntityMs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Invite> Invites { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource = C:\\Users\\jurin\\RiderProjects\\DCA1-VIA_Event_Association\\DbFile\\VeaDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasIndex(e => e.CreatorId, "IX_Events_CreatorId");

            entity.HasIndex(e => e.LocationId, "IX_Events_LocationId");

            entity.HasOne(d => d.Creator).WithMany(p => p.Events).HasForeignKey(d => d.CreatorId);

            entity.HasOne(d => d.Location).WithMany(p => p.Events).HasForeignKey(d => d.LocationId);
        });

        modelBuilder.Entity<Invite>(entity =>
        {
            entity.ToTable("Invite");

            entity.HasIndex(e => e.CreatorId, "IX_Invite_CreatorId");

            entity.HasIndex(e => e.EventId, "IX_Invite_EventId");

            entity.HasIndex(e => e.GuestId, "IX_Invite_GuestId");

            entity.HasOne(d => d.Creator).WithMany(p => p.Invites).HasForeignKey(d => d.CreatorId);

            entity.HasOne(d => d.Event).WithMany(p => p.Invites).HasForeignKey(d => d.EventId);

            entity.HasOne(d => d.Guest).WithMany(p => p.Invites).HasForeignKey(d => d.GuestId);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasIndex(e => e.CreatorId, "IX_Locations_CreatorId");

            entity.HasOne(d => d.Creator).WithMany(p => p.Locations).HasForeignKey(d => d.CreatorId);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Request");

            entity.HasIndex(e => new { e.EventId, e.GuestId }, "IX_Request_EventId_GuestId").IsUnique();

            entity.HasIndex(e => e.GuestId, "IX_Request_GuestId");

            entity.HasOne(d => d.Event).WithMany(p => p.Requests).HasForeignKey(d => d.EventId);

            entity.HasOne(d => d.Guest).WithMany(p => p.Requests).HasForeignKey(d => d.GuestId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    public VeaDbContext Seed()
    {
        var creator = new Creator
        {
            // Id = Guid.NewGuid().ToString(),
            Id = "bb99156b-67a9-46d1-9b3e-8584f7f827c3",
            Email = "creator@gmail.com"
        };
        Creators.Add(creator);
        SaveChanges();
        
        Locations.AddRange(LocationSeedFactory.CreateLocations(creator.Id));
        
        Events.AddRange(EventSeedFactory.CreateEvents(creator.Id));
        SaveChanges();
        
        Guests.AddRange(GuestSeedFactory.CreateGuests());
        SaveChanges();
        
        Invites.AddRange(InviteSeedFactory.CreateInvites(creator.Id));
        SaveChanges();
        
        Requests.AddRange(RequestSeedFactory.CreateRequests());
        SaveChanges();
        
        return this;
    }
}