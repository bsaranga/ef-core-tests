#nullable disable
using efcore_base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace efcore_base
{
    public class BloggingContext : DbContext
    {
        private string CONNECTION_STRING = "Host=localhost;Database=#db_name#;Username=root;Password=root";

        public BloggingContext(string dbname)
        {
            var split_conn_string = CONNECTION_STRING.Split('#');
            CONNECTION_STRING = split_conn_string[0] + dbname + split_conn_string[2];
            Console.WriteLine(CONNECTION_STRING);
        }

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
