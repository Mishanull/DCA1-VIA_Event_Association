using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationAggPersistence;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> entityTypeBuilder)
    {
        //Id : strongly-typed PK
        entityTypeBuilder.HasKey(l => l.Id);
        entityTypeBuilder
            .Property(l => l.Id)
            .HasConversion(
                lId => lId.Value,
                dbValue => TId.FromGuid<LocationId>(dbValue).Value!
            );

        //LocationName : non-nullable single-valued ValueObject
        entityTypeBuilder.ComplexProperty<LocationName>(
            location => location.Name,
            propertyBuilder =>
            {
                propertyBuilder.Property(locationName => locationName.Value)
                    .HasColumnName("LocationName")
                    .IsRequired();
            }
        );
        
        //FromTo : non-nullable single-valued ValueObject
        entityTypeBuilder.ComplexProperty<FromTo>(
            l => l.FromTo,
            propertyBuilder =>
            {
                propertyBuilder.Property(fromTo => fromTo.Start)
                    .HasColumnName("Start")
                    .IsRequired();
                propertyBuilder.Property(fromTo => fromTo.End)
                    .HasColumnName("End")
                    .IsRequired();
            }
        );
        
        //MaxGuests : non-nullable single-valued ValueObject
        entityTypeBuilder.ComplexProperty<MaxGuests>(
            l => l.MaxGuests,
            propertyBuilder =>
            {
                propertyBuilder.Property(maxGuests => maxGuests.Value)
                    .HasColumnName("MaxGuests")
                    .IsRequired();
            }
        );
        
        //CreatorId : strongly-typed FK
        entityTypeBuilder.HasOne<Creator>()
            .WithMany()
            .HasForeignKey(l=>l.CreatorId);
    }
}