using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EventAggPersistence;

public class EventEntityConfiguration : IEntityTypeConfiguration<VeaEvent>
{
    public void Configure(EntityTypeBuilder<VeaEvent> propertyTypeBuilder)
    {
        //Id : strongly-typed PK
        propertyTypeBuilder.HasKey(e => e.Id);
        propertyTypeBuilder.Property(e=>e.Id)
            .HasConversion(
                eId => eId.Value,
                dbValue => TId.FromGuid<VeaEventId>(dbValue).Value!
            );
        
        //Title : non-nullable single-valued ValueObject
        propertyTypeBuilder.ComplexProperty<Title>(
            e => e.Title,
            propertyBuilder =>
            {
                propertyBuilder.Property(t => t.Value)
                    .HasColumnName("Title");
            }
        );
        
        //Description : non-nullable single-valued ValueObject
        propertyTypeBuilder.ComplexProperty<Description>(
            e => e.Description,
            propertyBuilder =>
            {
                propertyBuilder.Property(d => d.Value)
                    .HasColumnName("Description");
            }
        );
        
        //VeaEventType : enum
        propertyTypeBuilder.Property<VeaEventType>(e => e.VeaEventType)
            .HasConversion(status => status.DisplayName,
                value => (VeaEventType)Enum.Parse(typeof(VeaEventType), value)
            );
        
        //MaxGuests : non-nullable single-valued ValueObject
        propertyTypeBuilder.ComplexProperty<MaxGuests>(
            e => e.MaxGuests,
            propertyBuilder =>
            {
                propertyBuilder.Property(m => m.Value)
                    .HasColumnName("MaxGuests");
            }
        );
        
        //VeaEventStatus : enum
        propertyTypeBuilder.Property<VeaEventStatus>(e => e.VeaEventStatus)
            .HasConversion(status => status.DisplayName,
                value => (VeaEventStatus)Enum.Parse(typeof(VeaEventStatus), value)
            );
        
        //FromTo : nullable multi-valued ValueObject
        propertyTypeBuilder.OwnsOne<FromTo>(e => e.FromTo, valueBuilder =>
            {
                valueBuilder.Property(ft => ft.Start)
                    .HasColumnName("From");
                valueBuilder.Property(ft => ft.End)
                    .HasColumnName("To");
            }
        );
        
        //CreatorId : strongly-typed foreign key
        propertyTypeBuilder.HasOne<Creator>()
            .WithMany()
            .HasForeignKey(e=>e.CreatorId);
        
        
        //LocationId : strongly-typed foreign key
        propertyTypeBuilder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(e=>e.LocationId);
    }
}