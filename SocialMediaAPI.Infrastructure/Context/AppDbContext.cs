using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Infrastructure.EntityTypeConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryViewer> StoryViewers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<UserRelationship> UserRelationships { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkPlace> WorkPlaces { get; set; }
        public DbSet<ReactionsStatus> ReactionsStatuses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Applies all the entity type configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserProfileTC).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
