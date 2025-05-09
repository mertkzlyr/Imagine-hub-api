using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<UserFollows> UserFollows { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserFollows>()
            .HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

        modelBuilder.Entity<UserFollows>()
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserFollows>()
            .HasOne(uf => uf.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(uf => uf.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PostLike>()
            .HasKey(pl => new { pl.UserId, pl.PostId });

        modelBuilder.Entity<PostLike>()
            .HasOne(pl => pl.User)
            .WithMany(u => u.LikedPosts)
            .HasForeignKey(pl => pl.UserId);

        modelBuilder.Entity<PostLike>()
            .HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId);
    } 
}