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
    public class WorkPlaceTC : IEntityTypeConfiguration<WorkPlace>
    {
        public void Configure(EntityTypeBuilder<WorkPlace> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(w => w.Profile)
                .WithMany(p => p.WorkPlaces)
                .HasForeignKey(w => w.ProfileId)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
