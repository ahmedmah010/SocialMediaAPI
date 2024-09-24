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
    public class UserRelationshipTC : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(up => up.RelationshipAsRequester)
                .WithOne(rp => rp.Requester)
                .HasForeignKey<UserRelationship>(rp => rp.RequesterId)
                .HasPrincipalKey<UserProfile>(up => up.Id)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(up => up.RelationshipAsPartner)
                .WithOne(rp => rp.Partner)
                .HasForeignKey<UserRelationship>(rp => rp.PartnerId)
                .HasPrincipalKey<UserProfile>(up => up.Id)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
