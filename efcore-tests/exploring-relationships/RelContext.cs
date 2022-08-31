using exploring_relationships.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace exploring_relationships
{
    public class RelContext : DbContext
    {
        private string CONNECTION_STRING = "Host=localhost;Database=#db_name#;Username=root;Password=root";

        public RelContext(string dbname)
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
            modelBuilder.Entity<Author>()
                        .HasMany(a => a.Books)
                        .WithMany(b => b.Authors)
                        .UsingEntity(j => j.ToTable("AuthorBooks"));
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
    }
}