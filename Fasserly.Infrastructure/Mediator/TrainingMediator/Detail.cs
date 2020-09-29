using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Detail
    {
        public class Query : IRequest<Training>
        {
            public Query(Guid id)
            {
                Id = id;
            }
            public Guid Id { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Query, Training>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }
            public async Task<Training> Handle(Query request, CancellationToken cancellationToken)
            {
                var training = await context.Trainings.FindAsync(request.Id);
                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                return training;
            }
        }
    }
}
