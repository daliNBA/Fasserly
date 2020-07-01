using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Fasserly.UnitTests.DataAccess
{
    [TestClass]
    public class TrainingDataServicesTest : DatabaseTestsBase
    {
        [TestMethod]
        public async Task GetAllTrainigAsync()
        {
            var training1 = RandomTraining(true);
            var training2 = RandomTraining(true);
            using (var context = new DatabaseContext(options))
            {
                context.Add(training1);
                context.Add(training2);
                context.SaveChanges();
            }
            using (var ds = new TrainingDataServices(options))
            {
                var trainings = await ds.GetAllTraining();
                Assert.AreEqual(3, trainings.Count());
            }
        }
    }
}
