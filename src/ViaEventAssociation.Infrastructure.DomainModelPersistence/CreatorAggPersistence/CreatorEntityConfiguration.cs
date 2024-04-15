using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.CreatorAggPersistence;

public class CreatorEntityConfiguration : IEntityTypeConfiguration<Creator>
{
    public void Configure(EntityTypeBuilder<Creator> entityTypeBuilder)
    {
        //Id : strongly-typed PK
        entityTypeBuilder.HasKey(c=>c.Id);
        entityTypeBuilder
            .Property(c => c.Id)
            .HasConversion(
                cId => cId.Value,
                dbValue => TId.FromGuid<CreatorId>(dbValue).Value!
            );
        
        //Email : non-nullable single-valued ValueObject
        entityTypeBuilder.ComplexProperty<Email>(
            creator => creator.Email,
            propertyBuilder =>
            {
                propertyBuilder.Property(email => email.Value)
                    .HasColumnName("Email")
                    .IsRequired();
            }
        );
    }
}