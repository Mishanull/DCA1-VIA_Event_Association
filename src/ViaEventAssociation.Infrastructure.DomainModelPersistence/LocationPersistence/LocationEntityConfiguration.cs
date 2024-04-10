using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.LocationAgg;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.LocationPersistence;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Id)
            .HasConversion(
                lId => lId.Value,
                dbValue => TId.FromGuid<LocationId>(dbValue).Value
            );
    }
}