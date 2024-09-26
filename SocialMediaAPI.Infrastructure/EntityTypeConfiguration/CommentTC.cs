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
    public class CommentTC : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(c => c.ParentComment)
                .WithMany(pc => pc.ChildComments)
                .HasForeignKey(c => c.ParentCommentId)
                .HasPrincipalKey(pc => pc.Id)
                .OnDelete(DeleteBehavior.NoAction); //Configure deletetion manually, as deleting a post deletes all the corresponding comments and deleteing a parent comment deletets all the corresponding comment which creates a cyclic cascade paths error (as there're two paths to delete the same entity "comment")
            builder
                .HasMany(c => c.Reactions)
                .WithOne(cr => cr.Comment)
                .HasForeignKey(cr => cr.CommentId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.NoAction); //Configure deletetion manually, as EF sees the ProductReaction and CommentReaction as One table (they're actually one table due to the TPH) which causes the cyclic cascade paths error
        }
    }
}
