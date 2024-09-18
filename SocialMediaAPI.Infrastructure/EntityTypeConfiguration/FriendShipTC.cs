using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.EntityTypeConfiguration
{
    public class FriendShipTC : IEntityTypeConfiguration<FriendShip>
    {
        public void Configure(EntityTypeBuilder<FriendShip> builder)
        {
            builder.HasKey(e => new { e.FriendId, e.UserId }); //Composite PK
            builder
                .HasOne(fs => fs.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(fs => fs.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder
              .HasOne(fs => fs.Friend)
              .WithMany() //No navigation property "Friends" to avoid circular reference error
              .HasForeignKey(fs => fs.FriendId)
              .HasPrincipalKey(u => u.Id)
              .OnDelete(DeleteBehavior.NoAction); //SQL Server doesn't allow multiple cascading paths for the same entity. As the cascade delete is already done above
        }
    }
}
