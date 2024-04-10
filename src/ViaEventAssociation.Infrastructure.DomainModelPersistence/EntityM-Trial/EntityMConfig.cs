using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EntityM_Trial;

public class EntityMConfig:IEntityTypeConfiguration<EntityM>
{
    public void Configure(EntityTypeBuilder<EntityM> builder)
    {
        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Id)
            .HasConversion(
                lId => lId.Value,
                dbValue => TId.FromGuid<MId>(dbValue).Value
            );
    }
}