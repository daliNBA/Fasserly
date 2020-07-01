using Fasserly.Database;
using Fasserly.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.DataAccess
{
    public class TrainingDataServices : BaseDataAccess
    {
        public TrainingDataServices(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public async Task AddTraining(Training training)
        {
            context.Trainings.Add(training);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Training>> GetAllTraining()
        {
            return await context.Trainings.Where(x => x.IsActive).ToListAsync();
        }
    }
}
