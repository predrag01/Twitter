using Azure.Core;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class TwitterContext: DbContext
    {
        public TwitterContext(DbContextOptions<TwitterContext> options) : base(options) { }

        public DbSet<User>? Users { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Like>? Likes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<FollowingList> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(fl => fl.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(fl => fl.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowingList>()
                .HasOne(fl => fl.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(fl => fl.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowingList>()
                .HasOne(fl => fl.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(fl => fl.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
