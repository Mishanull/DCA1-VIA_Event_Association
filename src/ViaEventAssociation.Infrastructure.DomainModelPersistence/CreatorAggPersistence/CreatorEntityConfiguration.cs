using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.CreatorAgg;

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
        
        // *Done directly in the WriteDbContext class
        // //CreatedInvites : list of entities
        // entityTypeBuilder.OwnsMany<Invite>(c => c.CreatedInvites, valueBuilder =>
        //     {
        //         valueBuilder.HasKey(i=>i.Id);
        //         valueBuilder.Property(i => i.Id)
        //             .HasConversion(
        //                 iId => iId.Value,
        //                 dbValue => TId.FromGuid<InviteId>(dbValue).Value!
        //             );
        //         valueBuilder.Property(i => i.InviteStatus)
        //             .HasConversion(
        //                 inviteStatus=>inviteStatus.ToString(),
        //                 dbValue => (InviteStatus)Enum.Parse(typeof(InviteStatus), dbValue)
        //             );
        //         valueBuilder.Property(i => i.Timestamp);
        //         valueBuilder.Property(i => i.CreatorId)
        //             .HasConversion(
        //                 cId => cId.Value,
        //                 dbValue => TId.FromGuid<CreatorId>(dbValue).Value!
        //             );
        //         valueBuilder.Property(i=>i.GuestId)
        //             .HasConversion(
        //                 gId => gId.Value,
        //                 dbValue => TId.FromGuid<GuestId>(dbValue).Value!
        //             );
        //         valueBuilder.Property(i=>i.EventId)
        //             .HasConversion(
        //                 eId => eId.Value,
        //                 dbValue => TId.FromGuid<VeaEventId>(dbValue).Value!
        //             );
        //     }
        // );
    }
}