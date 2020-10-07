using Fasserly.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fasserly.UnitTests.Database
{
    [TestClass]
    public class DatabaseContextTests : DatabaseTestsBase
    {
        [TestMethod]
        public async Task TestEFCoreModel1()
        {
            var training = RandomTraining();
            var category = RandomCategory();
            using (var context = new DatabaseContext(options))
            {
                context.Trainings.Add(training);
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
            using (var context = new DatabaseContext(options))
            {
                var e = await context.Trainings.FindAsync(training.TrainingId);
                var m = await context.Categories.FindAsync(category.CategoryId);
                //Assert.IsNull(e.category);
                Assert.AreEqual<int>(m.Trainings.Count, 0);
            }
        }

    }
}
