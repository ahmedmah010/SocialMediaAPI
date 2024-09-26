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
    public class FriendRequestTC : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(r => r.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(r => r.RequesterId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(r => r.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(r => r.ReceiverId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
