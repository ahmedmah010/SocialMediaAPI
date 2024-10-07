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
    public class PostTC : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(u => u.Id);
            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .HasPrincipalKey(p => p.Id);
            builder
                .HasMany(p=>p.Reactions)
                .WithOne(pr => pr.Post)
                .HasForeignKey(pr=>pr.PostId)
                .HasPrincipalKey(p=> p.Id)
                .OnDelete(DeleteBehavior.NoAction); //Configure deletetion manually, as EF sees the ProductReaction and CommentReaction as One table (they're actually one table due to the TPH) which causes the cyclic cascade paths error
            builder
                .HasMany(p => p.Media)
                .WithOne(ph => ph.Post)
                .HasForeignKey(ph => ph.PostId)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
