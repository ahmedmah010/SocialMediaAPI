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
    public class ReactionTC : IEntityTypeConfiguration<ReactionBase>
    {
        public void Configure(EntityTypeBuilder<ReactionBase> builder)
        {
            builder.ToTable("Reactions");
            builder.HasKey(e=>e.Id);
            builder
                .HasDiscriminator(e => e.IsPostReaction)
                .HasValue<PostReaction>(true)
                .HasValue<CommentReaction>(false);
            builder
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .HasPrincipalKey(u => u.Id);
        }
    }
}
