using efcore_base;
using efcore_base.Models;
using System.Reflection;
using Xunit.Abstractions;

namespace BloggingContextTests
{
    public class Basics
    {
        private readonly ITestOutputHelper testOutput;

        public Basics(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }

        [Fact]
        public void Seed()
        {
            var currentMethodName = MethodBase.GetCurrentMethod()?.Name;
            
            using var context = new BloggingContext(currentMethodName);
            
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Blogs.Add(new Blog()
            {
                Url = "https://learntree.io",
                Posts = new List<Post>
                {
                    new Post() { Title = "Hello WOrld", Content = "dhfksdjfh" }
                }
            });

            context.SaveChanges();

            var retrievedBlog = context.Blogs.Single(b => b.Url == "https://learntree.io");

            var expectedPostTitle = "Hello WOrld";
            var expectedPostContent = "dhfksdjfh";

            var actualPostTitle = retrievedBlog?.Posts?[0].Title;
            var actualPostContent = retrievedBlog?.Posts?[0].Content;

            var expected = (expectedPostTitle, expectedPostContent);
            var actual = (actualPostTitle, actualPostContent);

            Assert.Equal(expected, actual);
        }
    }
}