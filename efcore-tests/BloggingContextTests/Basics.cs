using efcore_base;
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
        public void SettingUpDbAndSeeding()
        {
            var currentMethodName = MethodInfo.GetCurrentMethod()?.Name;
            var context = new BloggingContext(currentMethodName);
            
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var canConnect = context.Database.CanConnect();
            Assert.True(canConnect);
            context.Dispose();

            Assert.True(true);
        }
    }
}