using efcore_base.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace efcore_base
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new BloggingContext();
            context.Database.EnsureCreated();

            context.Blogs.Add(new Blog { Url = "https://blog.learntree.io" });
            context.SaveChanges();
            Console.ReadKey();
        }
    }
}