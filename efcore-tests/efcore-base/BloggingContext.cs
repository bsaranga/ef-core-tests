#nullable disable
using efcore_base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace efcore_base
{
    public class BloggingContext : DbContext
    {
        private const string CONNECTION_STRING = "Host=localhost;Database=testdb_1;Username=root;Password=root";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(CONNECTION_STRING)
                          .EnableDetailedErrors(true)
                          .EnableSensitiveDataLogging(true)
                          .LogTo(Console.WriteLine, LogLevel.Debug, DbContextLoggerOptions.SingleLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(blog =>
            {
                blog.HasKey(b => b.BlogId);
                blog.HasIndex(b => b.Url)
                    .IsUnique()
                    .HasDatabaseName("BlogUrl_UNIQUE_IX");
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.PostId);
                post.HasIndex(p => new { p.PostId, p.BlogId, p.Title })
                    .IsUnique()
                    .HasDatabaseName("PostId_Blog_Title_UNIQUE_IX");
            });
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
