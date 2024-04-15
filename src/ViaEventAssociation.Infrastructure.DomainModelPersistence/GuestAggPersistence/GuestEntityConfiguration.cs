using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Common.Base;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.GuestAggPersistence;

public class GuestEntityConfiguration : IEntityTypeConfiguration<VeaGuest>
{
    public void Configure(EntityTypeBuilder<VeaGuest> propertyTypeBuilder)
    {
        //Id : strongly-typed PK
        propertyTypeBuilder.HasKey(g => g.Id);
        propertyTypeBuilder
            .Property(g => g.Id)
            .HasConversion(
                gId => gId.Value,
                dbValue => TId.FromGuid<GuestId>(dbValue).Value!
            );

        //Email : nullable single-valued ValueObject
        propertyTypeBuilder.OwnsOne<Email>(g => g.Email)
            .Property(e => e.Value)
            .HasColumnName("Email");
        
        //FirstName : nullable single-valued ValueObject
        propertyTypeBuilder.OwnsOne<FirstName>(g => g.FirstName)
            .Property(f => f.Value)
            .HasColumnName("FirstName");
        
        //LastName : nullable single-valued ValueObject
        propertyTypeBuilder.OwnsOne<LastName>(g => g.LastName)
            .Property(l => l.Value)
            .HasColumnName("LastName");
        
        //PictureUrl : nullable single-valued ValueObject
        propertyTypeBuilder.OwnsOne<PictureUrl>(g => g.PictureUrl)
            .Property(p => p.Value)
            .HasColumnName("PictureUrl");
    }
}