using Fasserly.Database;
using Fasserly.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Training> GetTrainingById(Guid Id)
        {
            return await context.Trainings.FindAsync(Id);
        }

        public async Task<Unit> CreateTraining(Training training)
        {
            context.Trainings.Add(training);
            var success = await context.SaveChangesAsync() > 0;
            if (success) return Unit.Value;
            throw new Exception("Saving problem");
        }
    }
}
