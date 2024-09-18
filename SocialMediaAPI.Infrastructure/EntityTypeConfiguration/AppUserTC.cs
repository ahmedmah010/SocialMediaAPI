using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.EntityTypeConfiguration
{
    public class AppUserTC : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.AccountStatus).HasDefaultValue(AccountStatus.Active);
            builder.Property(e => e.IsVerified).HasDefaultValue(false);
        }
    }
}
