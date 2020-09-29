using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class List
    {
        public class Query : IRequest<List<Training>> { }
        public class Handler : BaseDataAccess, IRequestHandler<Query, List<Training>>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }
            public async Task<List<Training>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Trainings.Where(x => x.IsActive).ToListAsync();
            }
        }
    }
}
