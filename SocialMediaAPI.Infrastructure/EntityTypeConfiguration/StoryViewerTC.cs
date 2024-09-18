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
    public class StoryViewerTC : IEntityTypeConfiguration<StoryViewer>
    {
        public void Configure(EntityTypeBuilder<StoryViewer> builder)
        {
            builder.HasKey(e => new { e.StoryId, e.ViewerId }); //Composite PK
            builder.Property(e => e.ViewedAt).HasDefaultValue(DateTime.Now);
            builder
                .HasOne(sv => sv.Viewer)
                .WithMany()
                .HasForeignKey(sv => sv.ViewerId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction); //Deleting a user must not cascade delete all the corresponding storyviews because this is handled when deleting a user all the corresponding stories are deleted, and when a story is deleted all the corresponding story views are deleted, thus, we handled the situation without causing the cyclic cascade paths error
            builder
                .HasOne(sv => sv.Story)
                .WithMany(s => s.Viewers)
                .HasForeignKey(sv => sv.StoryId)
                .HasPrincipalKey(s => s.Id); //By default, OnDelete behavior is set to cascade when not defined. Here, we're making the last point valid by cascade delete all the story views corresponding to a story if deleted

        }
    }
}
